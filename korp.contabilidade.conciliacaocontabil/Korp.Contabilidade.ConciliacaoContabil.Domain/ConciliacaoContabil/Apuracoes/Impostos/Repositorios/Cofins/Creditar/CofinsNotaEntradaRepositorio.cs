using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Extensoes;
using Microsoft.EntityFrameworkCore;
using Viasoft.Core.DDD.Repositories;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos.Repositorios.Cofins.Creditar;

public class CofinsNotaEntradaRepositorio: ICofinsCreditarRepositorio
{
    private readonly IRepository<FiscalNotaEntrada> _notaEntrada;
    private readonly IRepository<FiscalItemNotaEntrada> _itemNotaEntrada;

    public CofinsNotaEntradaRepositorio(IRepository<FiscalNotaEntrada> notaEntrada, IRepository<FiscalItemNotaEntrada> itemNotaEntrada)
    {
        _notaEntrada = notaEntrada;
        _itemNotaEntrada = itemNotaEntrada;
    }

    public async Task<IReadOnlyCollection<IImpostoDto>> ListarImpostoCreditar(ConciliacaoContabil conciliacaoContabil)
    {
        var notaEntrada = _notaEntrada.AsQueryable().AsNoTracking();
        var itemNotaEntrada = _itemNotaEntrada.AsQueryable().AsNoTracking();

        var notaEntradaEntrada = from e in notaEntrada.FiltrarPeriodoContabil(conciliacaoContabil)
            join c in itemNotaEntrada on e.LegacyId equals c.IdNotaEntrada
            where  c.ValorCofins > 0
            group c by new
            {
                e.Data,
                e.Documento,
                e.Serie,
                e.RazaoSocial,
                e.CodigoFornecedor,
                e.LegacyCompanyId,
            }
            into g
            select new ImpostoDto
            {
                Data = g.Key.Data,
                Documento = g.Key.Documento.ToString()+"/"+ g.Key.Serie.ToString(),
                RazaoSocial = g.Key.RazaoSocial,
                Valor = g.Sum(x => x.ValorCofins.Value),
                Codigo = g.Key.CodigoFornecedor,
                LegacyCompanyId = g.Key.LegacyCompanyId,
                Parcela = "1",
            };

        return await notaEntradaEntrada.AsNoTracking().ToListAsync();
    }
}