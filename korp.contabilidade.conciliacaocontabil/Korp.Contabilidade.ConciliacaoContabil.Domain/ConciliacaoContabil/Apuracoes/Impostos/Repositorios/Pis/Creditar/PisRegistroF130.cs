using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Pis.Creditar;

public class PisRegistroF130: IPisCreditarRepositorio
{
    private readonly IRepository<CreditoPis> _creditoPis;
    private readonly IRepository<PatrimonialBens> _patrimonialBens
        ;
    
    public PisRegistroF130(IRepository<CreditoPis> creditoPis, IRepository<PatrimonialBens> patrimonialBens)
    {
        _creditoPis = creditoPis;
        _patrimonialBens = patrimonialBens;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoCreditar(ConciliacaoContabil conciliacaoContabil)
    {
        var creditoPisQ = _creditoPis.AsQueryable().AsNoTracking();
        var patrimonialBens = _patrimonialBens.AsQueryable().AsNoTracking();

        var  registroF130 =
            from creditoPis in creditoPisQ
            join bem in patrimonialBens.FiltrarEmpresaPatrimonial(conciliacaoContabil) on creditoPis.LegacyIdBem equals bem.LegacyId
            where creditoPis.Ano == conciliacaoContabil.DataInicial.Year &&
                  creditoPis.Mes == conciliacaoContabil.DataInicial.Month &&
                  bem.OpcaoCreditoEfd == 1
            select new ImpostoDto
            {
                Data = new DateOnly(creditoPis.Ano, creditoPis.Mes, DateTime.DaysInMonth(creditoPis.Ano, creditoPis.Mes)),
                Valor = creditoPis.Valor,
                Documento = bem.CodigoBem + ' ' +  bem.Nome,
                Codigo = bem.Fornecedor,
                RazaoSocial = bem.RazaoSocial,
                LegacyCompanyId = bem.LegacyCompanyId
            };

        return await registroF130.AsNoTracking().ToListAsync();
    }
}