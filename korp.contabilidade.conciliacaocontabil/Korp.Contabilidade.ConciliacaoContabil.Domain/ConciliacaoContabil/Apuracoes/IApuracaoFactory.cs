namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes;

public interface IApuracaoFactory
{
    IApuracao CriarApuracao(ConciliacaoContabil apuracaoContabil);
}