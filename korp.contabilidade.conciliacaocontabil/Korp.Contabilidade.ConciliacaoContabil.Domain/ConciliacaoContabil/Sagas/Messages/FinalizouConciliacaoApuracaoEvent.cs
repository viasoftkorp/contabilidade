using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas.Messages;

[Endpoint("Korp.Contabilidade.ConciliacaoContabil.FinalizouConciliacaoApuracao")]
public class FinalizouConciliacaoApuracaoEvent: IEvent
{
    public Guid IdConciliacaoContabil { get; set; }
}