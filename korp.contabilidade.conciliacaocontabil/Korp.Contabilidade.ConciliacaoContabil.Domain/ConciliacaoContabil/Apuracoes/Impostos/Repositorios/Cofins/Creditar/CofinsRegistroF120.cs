using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar;

public class CofinsRegistroF120: ICofinsCreditarRepositorio
{
    private readonly IRepository<DepreciacaoConfiguracao> _depreciacaoConfiguracao;
    private readonly IRepository<DepreciacaoAceleradaValores> _depreciacaoAceleradaValores;
    private readonly IRepository<DepreciacaoGerencialValores> _depreciacaoGerencialValores;
    private readonly IRepository<DepreciacaoIncentivadaValores> _depreciacaoIncentivadaValores;
    private readonly IRepository<DepreciacaoLinearValores> _depreciacaoLinearValores;
    private readonly IRepository<PatrimonialBens> _patrimonialBens;
    private readonly IRepository<PatrimonialGrupoBem> _patrimonialGrupoBem;

    public CofinsRegistroF120(IRepository<DepreciacaoConfiguracao> depreciacaoConfiguracao, IRepository<DepreciacaoAceleradaValores> depreciacaoAceleradaValores, 
        IRepository<DepreciacaoGerencialValores> depreciacaoGerencialValores, IRepository<DepreciacaoIncentivadaValores> depreciacaoIncentivadaValores, 
        IRepository<DepreciacaoLinearValores> depreciacaoLinearValores, IRepository<PatrimonialBens> patrimonialBens, IRepository<PatrimonialGrupoBem> patrimonialGrupoBem)
    {
        _depreciacaoConfiguracao = depreciacaoConfiguracao;
        _depreciacaoAceleradaValores = depreciacaoAceleradaValores;
        _depreciacaoGerencialValores = depreciacaoGerencialValores;
        _depreciacaoIncentivadaValores = depreciacaoIncentivadaValores;
        _depreciacaoLinearValores = depreciacaoLinearValores;
        _patrimonialBens = patrimonialBens;
        _patrimonialGrupoBem = patrimonialGrupoBem;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoCreditar(ConciliacaoContabil conciliacaoContabil)
    {
         var depreciacaoConfiguracao = _depreciacaoConfiguracao.AsQueryable().AsNoTracking();
        var depreciacaoAceleradaValores = _depreciacaoAceleradaValores.AsQueryable().AsNoTracking();
        var depreciacaoGerencialValores = _depreciacaoGerencialValores.AsQueryable().AsNoTracking();
        var depreciacaoIncentivadaValores = _depreciacaoIncentivadaValores.AsQueryable().AsNoTracking();
        var depreciacaoLinearValores = _depreciacaoLinearValores.AsQueryable().AsNoTracking();
        var patrimonialBens = _patrimonialBens.AsQueryable().AsNoTracking();
        var patrimonialGrupoBem = _patrimonialGrupoBem.AsQueryable().AsNoTracking();
        var registroF120  = (
                from linear in depreciacaoLinearValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on linear.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on linear.LegacyIdBem equals bem.LegacyId
                join grupo in patrimonialGrupoBem on bem.CodigoGrupoBem equals grupo.Codigo
                where configuracao.TipoContabilidade == "Dep. Linear" &&
                      (bem.EfdpcGerarBem ?? false) == false &&
                      bem.OpcaoCreditoEfd == 2 &&
                      linear.ValorCofins > 0  
                select new
                {
                    linear.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    ValorCofinsF120 = linear.ValorCofins
                }
            ).Union(
                from acelerada in depreciacaoAceleradaValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on acelerada.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on acelerada.LegacyIdBem equals bem.LegacyId
                join grupo in patrimonialGrupoBem on bem.CodigoGrupoBem equals grupo.Codigo
                where configuracao.TipoContabilidade == "Dep. Acelerada" &&
                      (bem.EfdpcGerarBem ?? false) == false && 
                      bem.OpcaoCreditoEfd == 2 &&
                      acelerada.ValorCofins > 0  
                select new
                {
                    acelerada.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    ValorCofinsF120 = acelerada.ValorCofins
                }
            ).Union(
                from incentivada in depreciacaoIncentivadaValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on incentivada.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on incentivada.LegacyIdBem equals bem.LegacyId
                join grupo in patrimonialGrupoBem on bem.CodigoGrupoBem equals grupo.Codigo
                where configuracao.TipoContabilidade == "Dep. Incentivada" &&
                      (bem.EfdpcGerarBem ?? false) == false && 
                      bem.OpcaoCreditoEfd == 2 &&
                      incentivada.ValorCofins > 0  
                select new
                {
                    incentivada.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    ValorCofinsF120 = incentivada.ValorCofins
                }
            ).Union(
                from gerencial in depreciacaoGerencialValores.FiltrarPeriodoDepreciacao(conciliacaoContabil)
                join configuracao in depreciacaoConfiguracao on gerencial.LegacyIdBem equals configuracao.LegacyIdBem
                join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on gerencial.LegacyIdBem equals bem.LegacyId
                join grupo in patrimonialGrupoBem on bem.CodigoGrupoBem equals grupo.Codigo
                where configuracao.TipoContabilidade == "Dep. Gerencial" &&
                      (bem.EfdpcGerarBem ?? false) == false && 
                      bem.OpcaoCreditoEfd == 2 &&
                      gerencial.ValorCofins > 0   
                select new
                {
                    gerencial.Data,
                    bem.Fornecedor,
                    bem.CodigoBem,
                    bem.Nome,
                    bem.LegacyCompanyId,
                    ValorCofinsF120 = gerencial.ValorCofins
                }
            ).GroupBy(x => new { x.Data, x.Fornecedor, x.CodigoBem, x.Nome, x.LegacyCompanyId })
            .Select(g => new ImpostoDto
            {
                Data = g.Key.Data,
                Documento = g.Key.CodigoBem + ' ' +  g.Key.Nome,
                Codigo = g.Key.Fornecedor,
                Valor = g.Sum(x => x.ValorCofinsF120),
                LegacyCompanyId = g.Key.LegacyCompanyId
            });

        return await registroF120.AsNoTracking().ToListAsync();
    }
}