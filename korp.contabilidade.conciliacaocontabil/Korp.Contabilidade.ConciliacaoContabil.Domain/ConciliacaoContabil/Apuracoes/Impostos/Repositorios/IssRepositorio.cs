using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios;

public class IssRepositorio: IImpostoRepositorio
{
    private readonly IRepository<FiscalNotaSaida> _notaSaida;
    private readonly IRepository<FiscalItemNotaSaida> _itemNotaSaida;

    public IssRepositorio(IRepository<FiscalNotaSaida> notaSaida, IRepository<FiscalItemNotaSaida> itemNotaSaida)
    {
        _notaSaida = notaSaida;
        _itemNotaSaida = itemNotaSaida;
    }

    public Task<IReadOnlyCollection<IImpostoDto>> ListarNotaEntrada(ConciliacaoContabil conciliacaoContabil)
    {
        throw new System.NotImplementedException();
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarNotaSaida(ConciliacaoContabil conciliacaoContabil)
    {
        var notaSaida = _notaSaida.AsQueryable().AsNoTracking();
        var itemNotaSaida = _itemNotaSaida.AsQueryable().AsNoTracking();

        var notaSaidaSaida = from e in notaSaida.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaSaida on e.LegacyId equals c.IdNotaSaida
            where c.ValorIss > 0
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
            select new NotaSaidaDto
            {
                Data = g.Key.Data,
                Documento = g.Key.Documento.ToString()+"/"+ g.Key.Serie.ToString(),
                RazaoSocial = g.Key.RazaoSocial,
                Valor = g.Sum(x => x.ValorIss.Value) * -1,
                Codigo = g.Key.CodigoCliente,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };
        
        var apuracoesFiscalNotaSaida = await notaSaidaSaida.AsNoTracking().ToListAsync();

        return apuracoesFiscalNotaSaida;
    }
}