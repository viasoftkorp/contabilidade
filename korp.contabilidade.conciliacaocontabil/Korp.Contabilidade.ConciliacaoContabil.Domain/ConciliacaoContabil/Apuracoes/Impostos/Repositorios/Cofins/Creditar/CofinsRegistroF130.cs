using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar;

public class CofinsRegistroF130: ICofinsCreditarRepositorio
{
    private readonly IRepository<CreditoCofins> _creditoCofins;
    private readonly IRepository<PatrimonialBens> _patrimonialBens
        ;
    
    public CofinsRegistroF130(IRepository<CreditoCofins> creditoCofins, IRepository<PatrimonialBens> patrimonialBens)
    {
        _creditoCofins = creditoCofins;
        _patrimonialBens = patrimonialBens;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoCreditar(ConciliacaoContabil conciliacaoContabil)
    {
        var creditoCofinsQ = _creditoCofins.AsQueryable().AsNoTracking();
        var patrimonialBens = _patrimonialBens.AsQueryable().AsNoTracking();

        var  registroF130 =
            from creditoCofins in creditoCofinsQ
            join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on creditoCofins.LegacyIdBem equals bem.LegacyId
            where creditoCofins.Ano == conciliacaoContabil.DataInicial.Year &&
                  creditoCofins.Mes == conciliacaoContabil.DataInicial.Month &&
                  bem.OpcaoCreditoEfd == 1
            select new ImpostoDto
            {
                Data = new DateOnly(creditoCofins.Ano, creditoCofins.Mes, DateTime.DaysInMonth(creditoCofins.Ano, creditoCofins.Mes)),
                Valor = creditoCofins.Valor,
                Documento = bem.CodigoBem + ' ' +  bem.Nome,
                Codigo = bem.Fornecedor,
                RazaoSocial = bem.RazaoSocial,
                LegacyCompanyId = bem.LegacyCompanyId
            };

        return await registroF130.AsNoTracking().ToListAsync();
    }
}