using System;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Configuracoes.Dtos;

public class ConfiguracaoDto : EntityDto
{
    public Guid IdOperacaoContabilAdiantamento { get; set; }
    public Guid? IdContaContabilPai { get; set; }
}
