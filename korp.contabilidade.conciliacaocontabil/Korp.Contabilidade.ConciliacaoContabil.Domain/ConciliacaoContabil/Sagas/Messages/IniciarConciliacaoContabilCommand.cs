using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas.Messages;

[Endpoint("Korp.Contabilidade.ConciliacaoContabil.IniciarConciliacaoContabil")]
public class IniciarConciliacaoContabilCommand: ICommand
{
    public Guid IdConciliacaoContabil { get; set; }
}