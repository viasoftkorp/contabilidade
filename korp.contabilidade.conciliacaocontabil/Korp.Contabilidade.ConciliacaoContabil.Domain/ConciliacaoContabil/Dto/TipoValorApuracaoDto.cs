namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class TipoValorApuracaoDto
{
    public int Id { get; set; }
    public string Description { get; set; }

    public TipoValorApuracaoDto()
    {
    }

    public TipoValorApuracaoDto(int id, string description)
    {
        Id = id;
        Description = description;
    }
}