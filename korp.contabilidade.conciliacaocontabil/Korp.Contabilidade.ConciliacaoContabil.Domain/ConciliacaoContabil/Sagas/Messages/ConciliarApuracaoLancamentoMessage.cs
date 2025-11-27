using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas.Messages;

[Endpoint("Korp.Contabilidade.ConciliacaoContabil.ConciliarApuracaoLancamento")]
public class ConciliarApuracaoLancamentoMessage: IMessage
{
    public Guid IdConciliacaoContabil { get; set; }  
}