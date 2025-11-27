using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Patrimonial.Repositorio;

public class ValorImobilizadoPatrimonial: IPatrimonialRepositorio
{
    private readonly IRepository<PatrimonialBens> _patrimonialBens;
    private readonly IRepository<Fornecedor> _fornecedor;
    private readonly IRepository<DepreciacaoConfiguracao> _depreciacaoConfiguracao;
    private readonly IRepository<DepreciacaoAceleradaValores> _depreciacaoAceleradaValores;
    private readonly IRepository<DepreciacaoGerencialValores> _depreciacaoGerencialValores;
    private readonly IRepository<DepreciacaoIncentivadaValores> _depreciacaoIncentivadaValores;
    private readonly IRepository<DepreciacaoLinearValores> _depreciacaoLinearValores;
    private readonly IRepository<PatrimonialItensVinculados> _patrimonialItensVinculados;
    private readonly IRepository<DepreciacaoReavaliacaoLinear> _depreciacaoReavaliacaoLinear;
    private readonly IRepository<DepreciacaoReavaliacaoGerencial> _depreciacaoReavaliacaoGerencial;
    private readonly IRepository<PatrimonialReavaliacao> _patrimonialReavaliacao;
    public ValorImobilizadoPatrimonial(IRepository<PatrimonialBens> patrimonialBens, IRepository<Fornecedor> fornecedor, IRepository<DepreciacaoConfiguracao> depreciacaoConfiguracao, IRepository<DepreciacaoAceleradaValores> depreciacaoAceleradaValores, IRepository<DepreciacaoGerencialValores> depreciacaoGerencialValores, IRepository<DepreciacaoIncentivadaValores> depreciacaoIncentivadaValores, IRepository<DepreciacaoLinearValores> depreciacaoLinearValores, IRepository<PatrimonialItensVinculados> patrimonialItensVinculados, IRepository<DepreciacaoReavaliacaoLinear> depreciacaoReavaliacaoLinear, IRepository<DepreciacaoReavaliacaoGerencial> depreciacaoReavaliacaoGerencial, IRepository<PatrimonialReavaliacao> patrimonialReavaliacao)
    {
        _patrimonialBens = patrimonialBens;
        _fornecedor = fornecedor;
        _depreciacaoConfiguracao = depreciacaoConfiguracao;
        _depreciacaoAceleradaValores = depreciacaoAceleradaValores;
        _depreciacaoGerencialValores = depreciacaoGerencialValores;
        _depreciacaoIncentivadaValores = depreciacaoIncentivadaValores;
        _depreciacaoLinearValores = depreciacaoLinearValores;
        _patrimonialItensVinculados = patrimonialItensVinculados;
        _depreciacaoReavaliacaoLinear = depreciacaoReavaliacaoLinear;
        _depreciacaoReavaliacaoGerencial = depreciacaoReavaliacaoGerencial;
        _patrimonialReavaliacao = patrimonialReavaliacao;
    }

    public async Task<IReadOnlyCollection<PatrimonialDto>> ListarBensPatrimonial(ConciliacaoContabil conciliacaoContabil)
    {
        var bens = _patrimonialBens.AsQueryable().AsNoTracking();
        var patrimonialItensVinculados = _patrimonialItensVinculados.AsQueryable().AsNoTracking();
        var fornecedor = _fornecedor.AsQueryable().AsNoTracking();
        var reavaliacao = _patrimonialReavaliacao.AsQueryable().AsNoTracking();
        
        var bensPatrimonial = from c in bens.FiltrarPeriodoEntradaPatrimonial(conciliacaoContabil)
            join f in fornecedor on c.Fornecedor equals f.Codigo into fornecedorGroup
            from f in fornecedorGroup.DefaultIfEmpty() 
            select new PatrimonialDto
            {
                Data = c.DataEntrada.Value,
                Documento = c.CodigoBem + " "+ c.Nome ,
                Valor = c.Valor,
                Codigo = c.Fornecedor,
                RazaoSocial = f.RazaoSocial,
                LegacyCompanyId = c.LegacyCompanyId,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.ValorImobilizadosPatrimonial,
                Parcela = "1"
            };
     
        var bensPatrimonialBaixado = from c in bens.FiltrarPeriodoSaidaPatrimonial(conciliacaoContabil)
            join f in fornecedor on c.Fornecedor equals f.Codigo into fornecedorGroup
            from f in fornecedorGroup.DefaultIfEmpty() 
            where c.ValorSaida > 0
            select new PatrimonialDto
            {
                Data = c.DataSaida.Value,
                Documento = c.CodigoBem + " "+ c.Nome ,
                Valor = c.ValorSaida * -1,
                Codigo = c.Fornecedor,
                RazaoSocial = f.RazaoSocial,
                LegacyCompanyId = c.LegacyCompanyId,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.ValorImobilizadosPatrimonial,
                Parcela = "1"
            };
        
        var bensPatrimonialItensBaixados = from c in patrimonialItensVinculados.FiltrarDataPatrimonial(conciliacaoContabil)
            join b in bens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on c.LegacIdBem equals b.LegacyId
            select new PatrimonialDto
            {
                Data = c.Data,
                Documento = b.CodigoBem + " "+ b.Nome ,
                Valor = c.Valor * -1,
                Codigo = c.CodigoFornecedorCliente,
                RazaoSocial = c.RazaoFornecedorCliente,
                LegacyCompanyId = b.LegacyCompanyId,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.ValorImobilizadosPatrimonial,
                Parcela = "1"
            };
        
        var bensPatrimonialReavaliacao = from c in reavaliacao.FiltrarDataPatrimonial(conciliacaoContabil)
            join b in bens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on c.CodigoBem equals b.CodigoBem
            select new PatrimonialDto
            {
                Data = c.Data,
                Documento = b.CodigoBem + " "+ b.Nome ,
                Valor = c.Operacao ? c.Valor :  c.Valor * -1,
                LegacyCompanyId = b.LegacyCompanyId,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.ValorImobilizadosPatrimonial,
                Parcela = "1"
            };
        var depreciacaoPatrimonial = RetornarDepreciacaoPatrimonial(conciliacaoContabil);
        var depreciacaoReavaliacaoLinear = RetornarDepreciacaoReavaliacaoLinear(conciliacaoContabil);
        var depreciacaoReavaliacaoGerencial = RetornarDepreciacaoReavaliacaoGerencial(conciliacaoContabil);

        var bensDto = await bensPatrimonial.AsNoTracking().ToListAsync();
        bensDto.AddRange(await bensPatrimonialBaixado.AsNoTracking().ToListAsync());
        bensDto.AddRange(await bensPatrimonialItensBaixados.AsNoTracking().ToListAsync());
        bensDto.AddRange(await depreciacaoPatrimonial.AsNoTracking().ToListAsync());
        bensDto.AddRange(await depreciacaoReavaliacaoLinear.AsNoTracking().ToListAsync());
        bensDto.AddRange(await depreciacaoReavaliacaoGerencial.AsNoTracking().ToListAsync());
        bensDto.AddRange(await bensPatrimonialReavaliacao.AsNoTracking().ToListAsync());
        return bensDto;
    }

    public IQueryable<PatrimonialDto> RetornarDepreciacaoReavaliacaoLinear(ConciliacaoContabil conciliacaoContabil)
    {
        var patrimonialBens = _patrimonialBens.AsQueryable().AsNoTracking();
        var depreciacaoReavaliacaoLinear = _depreciacaoReavaliacaoLinear.AsQueryable().AsNoTracking();

        return from reavaliacaoLinear in depreciacaoReavaliacaoLinear.FiltrarPeriodoDepreciacao(conciliacaoContabil)
            join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on reavaliacaoLinear.LegacyIdBem equals bem.LegacyId
            select new PatrimonialDto
            {
                Data = reavaliacaoLinear.Data,
                Codigo =bem.Fornecedor,
                Documento = bem.CodigoBem+ ' ' + bem.Nome,
                LegacyCompanyId = bem.LegacyCompanyId,
                Valor = reavaliacaoLinear.Valor,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.DepreciacaoPatrimonial
            };
    }
    
    public IQueryable<PatrimonialDto> RetornarDepreciacaoReavaliacaoGerencial(ConciliacaoContabil conciliacaoContabil)
    {
        var patrimonialBens = _patrimonialBens.AsQueryable().AsNoTracking();
        var depreciacaoReavaliacaoGerencial = _depreciacaoReavaliacaoGerencial.AsQueryable().AsNoTracking();

        return from gerencial in depreciacaoReavaliacaoGerencial.FiltrarPeriodoDepreciacao(conciliacaoContabil)
            join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on gerencial.LegacyIdBem equals bem.LegacyId
            select new PatrimonialDto
            {
                Data = gerencial.Data,
                Codigo =bem.Fornecedor,
                Documento = bem.CodigoBem+ ' ' + bem.Nome,
                LegacyCompanyId = bem.LegacyCompanyId,
                Valor = gerencial.Valor,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.DepreciacaoPatrimonial
            };
    }
    public IQueryable<PatrimonialDto> RetornarDepreciacaoPatrimonial(ConciliacaoContabil conciliacaoContabil)
    {
        var depreciacaoConfiguracao = _depreciacaoConfiguracao.AsQueryable().AsNoTracking();
        var depreciacaoAceleradaValores = _depreciacaoAceleradaValores.AsQueryable().AsNoTracking();
        var depreciacaoGerencialValores = _depreciacaoGerencialValores.AsQueryable().AsNoTracking();
        var depreciacaoIncentivadaValores = _depreciacaoIncentivadaValores.AsQueryable().AsNoTracking();
        var depreciacaoLinearValores = _depreciacaoLinearValores.AsQueryable().AsNoTracking();
        var patrimonialBens = _patrimonialBens.AsQueryable().AsNoTracking();
        return (
                from linear in depreciacaoLinearValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on linear.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on linear.LegacyIdBem equals bem.LegacyId
                where configuracao.TipoContabilidade == "Dep. Linear" &&
                      linear.Valor > 0  
                select new
                {
                    linear.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    linear.Valor
                }
            ).Union(
                from acelerada in depreciacaoAceleradaValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on acelerada.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on acelerada.LegacyIdBem equals bem.LegacyId
                where configuracao.TipoContabilidade == "Dep. Acelerada" &&
                      acelerada.Valor > 0  
                select new
                {
                    acelerada.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    acelerada.Valor
                }
            ).Union(
                from incentivada in depreciacaoIncentivadaValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on incentivada.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on incentivada.LegacyIdBem equals bem.LegacyId
                where configuracao.TipoContabilidade == "Dep. Incentivada" &&
                      incentivada.Valor > 0  
                select new
                {
                    incentivada.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    incentivada.Valor
                }
            ).Union(
                from gerencial in depreciacaoGerencialValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on gerencial.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on gerencial.LegacyIdBem equals bem.LegacyId
                where configuracao.TipoContabilidade == "Dep. Gerencial" &&
                    gerencial.Valor > 0   
                select new
                {
                    gerencial.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    gerencial.Valor
                }
            ).GroupBy(x => new { x.Data, x.Fornecedor, x.CodigoBem, x.Nome, x.LegacyCompanyId })
            .Select(g => new PatrimonialDto
            {
                Data = g.Key.Data,
                Documento = g.Key.CodigoBem + ' ' +  g.Key.Nome,
                Codigo = g.Key.Fornecedor,
                Valor = g.Sum(x => x.Valor) * -1,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                TipoValorApuracao = TipoValorApuracaoConciliacaoContabil.DepreciacaoPatrimonial
            });   
    }
    
}