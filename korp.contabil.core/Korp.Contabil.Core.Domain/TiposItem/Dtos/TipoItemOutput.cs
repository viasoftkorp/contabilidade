using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Korp.Contabil.Core.Domain.TiposItem.Dtos;

public class TipoItemOutput : EntityDto
{
    public string Codigo { get; set; }
    public string Descricao { get; set; }

    public TipoItemOutput() { }

    public TipoItemOutput(TipoItem tipoItem)
    {
        Id = tipoItem.Id;
        Codigo = tipoItem.Codigo;
        Descricao = tipoItem.Descricao;
    }
}
