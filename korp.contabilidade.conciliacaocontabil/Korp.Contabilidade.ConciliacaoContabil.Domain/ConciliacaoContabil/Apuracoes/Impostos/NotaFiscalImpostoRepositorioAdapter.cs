using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Impostos;

public class NotaFiscalImpostoRepositorioAdapter: INotaFiscalImpostoRepositorio
{
    private readonly TipoFiscal _fiscal;
    private readonly IImpostoRepositorio _impostoRepositorio;
    
    public NotaFiscalImpostoRepositorioAdapter(TipoFiscal fiscal, IImpostoRepositorio impostoRepositorio)
    {
        _fiscal = fiscal;
        _impostoRepositorio = impostoRepositorio;
    }
    
    public Task<IReadOnlyCollection<IImpostoDto>> ListarNotasFiscais(ConciliacaoContabil conciliacaoContabil)
    {
        return _fiscal switch
        {
            TipoFiscal.Credito => _impostoRepositorio.ListarNotaEntrada(conciliacaoContabil),
            TipoFiscal.Debito => _impostoRepositorio.ListarNotaSaida(conciliacaoContabil),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}