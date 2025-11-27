using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.ApuracoesEntidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Creditar;

public class PisRegistroF100: IPisCreditarRepositorio
{
    private readonly IRepository<ApuracoesEntidades.LancamentoContabil> _lancamentoContabil;
    private readonly IRepository<CabecalhoLancamentoContabil> _cabecalhoLancamentoContabil;
    private readonly IRepository<RegistroF100> _registroF100;
    private readonly IRepository<RegistroF100Contas> _registroF100Contas;
    private readonly IRepository<Fornecedor> _fornecedor;

    public PisRegistroF100(IRepository<RegistroF100> registroF100, IRepository<Fornecedor> fornecedor, IRepository<ApuracoesEntidades.LancamentoContabil> lancamentoContabil, 
        IRepository<CabecalhoLancamentoContabil> cabecalhoLancamentoContabil, IRepository<RegistroF100Contas> registroF100Contas)
    {
        _registroF100 = registroF100;
        _fornecedor = fornecedor;
        _lancamentoContabil = lancamentoContabil;
        _cabecalhoLancamentoContabil = cabecalhoLancamentoContabil;
        _registroF100Contas = registroF100Contas;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoCreditar(ConciliacaoContabil conciliacaoContabil)
    {
        var lancamentoContabil = _lancamentoContabil.AsQueryable().AsNoTracking();
        var cabecalhoLancamentoContabil = _cabecalhoLancamentoContabil.AsQueryable().AsNoTracking();
        var registroF100 = _registroF100.AsQueryable().AsNoTracking();
        var registroF100Contas = _registroF100Contas.AsQueryable().AsNoTracking();
        var fornecedor = _fornecedor.AsQueryable().AsNoTracking();
        
        var registrosF100 = from r in registroF100.FiltrarPeriodoContabil(conciliacaoContabil)
            join f in fornecedor on r.CodigoFornecedor equals f.Codigo
            where r.IndicadorOperacao == 0 && r.ValorPis > 0
            select new ImpostoDto
            {
                Data = r.Data,
                Documento = r.DescricaoOperacao,
                Codigo = r.CodigoFornecedor,
                RazaoSocial = f.RazaoSocial,
                Valor = r.ValorPis,
                LegacyCompanyId = r.LegacyCompanyId,
                Parcela = "1",
            };
        
        var registrosF100Contas = from lan in lancamentoContabil
            join cab in cabecalhoLancamentoContabil.FiltrarPeriodoContabil(conciliacaoContabil) on lan.NumeroLancamento equals cab.NumeroLancamento
            join contas in registroF100Contas on lan.CodigoConta equals contas.CodigoConta
            join f100 in registroF100 on contas.LegacyRegistroF100 equals f100.LegacyId
            join forn in fornecedor on f100.CodigoFornecedor equals forn.Codigo
            where f100.ParametrizacaoLancamento
            group new { lan, contas } by new 
            {
                f100.DescricaoOperacao,
                cab.Data,
                f100.LegacyCompanyId,
                f100.CodigoFornecedor,
                forn.RazaoSocial,
                f100.AliquotaPis,
                contas.Operacao
            } into g
            where g.Key.Operacao == "C"
                ? g.Sum(x => x.lan.ValorCredito) > 0
                : g.Sum(x => x.lan.ValorDebito) > 0
            select new ImpostoDto
            {
                Data = g.Key.Data,
                Documento = g.Key.DescricaoOperacao,
                Codigo = g.Key.CodigoFornecedor,
                RazaoSocial = g.Key.RazaoSocial,
                Valor = g.Key.Operacao == "C"
                    ? g.Sum(x => x.lan.ValorCredito)* (g.Key.AliquotaPis / 100)
                    : g.Sum(x => x.lan.ValorDebito) * (g.Key.AliquotaPis / 100),
                LegacyCompanyId = g.Key.LegacyCompanyId
            };
        var apuracoesImposto = await registrosF100.AsNoTracking().ToListAsync();
        apuracoesImposto.AddRange(await registrosF100Contas.AsNoTracking().ToListAsync());
        return apuracoesImposto;
    }
}