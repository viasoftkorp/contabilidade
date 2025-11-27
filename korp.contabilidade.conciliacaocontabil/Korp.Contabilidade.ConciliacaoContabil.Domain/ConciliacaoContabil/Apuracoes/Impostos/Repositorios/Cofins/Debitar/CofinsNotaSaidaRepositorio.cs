using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Debitar;

public class CofinsNotaSaidaRepositorio: ICofinsDebitarRepositorio
{
    private readonly IRepository<FiscalNotaSaida> _notaSaida;
    private readonly IRepository<FiscalItemNotaSaida> _itemNotaSaida;

    public CofinsNotaSaidaRepositorio(IRepository<FiscalNotaSaida> notaSaida, IRepository<FiscalItemNotaSaida> itemNotaSaida)
    {
        _notaSaida = notaSaida;
        _itemNotaSaida = itemNotaSaida;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoDebitar(ConciliacaoContabil conciliacaoContabil)
    {
        var notaSaida = _notaSaida.AsQueryable().AsNoTracking();
        var itemNotaSaida = _itemNotaSaida.AsQueryable().AsNoTracking();
        var notasSaida = from e in notaSaida.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaSaida on e.LegacyId equals c.IdNotaSaida
            where c.ValorCofins > 0 && e.Situacao == "IMPRESSA"
            group c by new
            {
                e.Data,
                e.Documento,
                e.Serie,
                e.RazaoSocial,
                e.CodigoCliente,
                e.LegacyCompanyId,
            }
            into g
            select new ImpostoDto
            {
                Data = g.Key.Data,
                Documento = g.Key.Documento.ToString()+"/"+ g.Key.Serie.ToString(),
                RazaoSocial = g.Key.RazaoSocial,
                Valor = g.Sum(x => x.ValorCofins.Value) * -1,
                Codigo = g.Key.CodigoCliente,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };

        return await notasSaida.AsNoTracking().ToListAsync();
    }
}