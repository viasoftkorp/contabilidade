namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Entidades;

public class ConciliacaoContabilErro
{
    public const string RecursoNaoEncontrado = "RecursoNaoEncontrado";
    
    public string Mensagem { get; set; }
    public string Codigo { get; set; }
}