using Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Apuracoes.Entidades;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Dto;

public class EmpresaDto : EntityDto
{
    public EmpresaDto()
    {
    }

    public EmpresaDto(Empresa empresa)
    {
        Id = empresa.Id;
        LegacyId = empresa.LegacyId;
        Nome = empresa.Nome;
    }

    public int LegacyId { get; set; }
    public string Nome { get; set; }
}