using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Repositorio;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Viasoft.Core.IoC.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.LancamentoContabil.Servicos;

public class LancamentoContabilService : ILancamentoContabilService, ITransientDependency
{
    private readonly ILancamentoContabilRepositorio _lancamentoContabilRepositorio;

    public LancamentoContabilService(ILancamentoContabilRepositorio lancamentoContabilRepositorio)
    {
        _lancamentoContabilRepositorio = lancamentoContabilRepositorio;
    }

    public async Task<List<ConciliacaoContabilLancamento>> ApurarLancamentoContabil(ConciliacaoContabil conciliacaoContabil)
    {
        var conciliarLancamentos = new List<ConciliacaoContabilLancamento>();
        var lancamentos = await _lancamentoContabilRepositorio.ListarLancamentosContabeis(conciliacaoContabil);
        foreach (var lancamento in lancamentos)
        {
            var conciliacaoContabilLancamento = new ConciliacaoContabilLancamento
            {
                NumeroLancamento = lancamento.NumeroLancamento,
                LegacyCompanyId = lancamento.LegacyCompanyId,
                Data = lancamento.DataLancamento,
                Historico = lancamento.Historico,
                Valor = lancamento.Valor,
                CodigoConta = lancamento.CodigoConta,
                Conciliado = false,
                CodigoFornecedorCliente = lancamento.CodigoFornecedorCliente,
                IdConciliacaoContabil = conciliacaoContabil.LegacyId,
                IdTipoLancamento = lancamento.IdTipoLancamento
            };
            conciliacaoContabilLancamento.GerarChaveConciliacao();
            conciliarLancamentos.Add(conciliacaoContabilLancamento);
        }
        
        return conciliarLancamentos;
    }
}