namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public enum AdicionarContaResponseEnum
{
    Ok = 0,
    ConciliacaoNaoEncontrada = 1,
    ContaJaAdicionada = 2,
    ContaVirtualSemAcaoDefinida = 3
}