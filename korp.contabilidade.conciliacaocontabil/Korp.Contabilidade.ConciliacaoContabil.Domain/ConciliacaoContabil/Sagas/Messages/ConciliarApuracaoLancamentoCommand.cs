using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Korp.Contabilidade.ConciliacaoContabil.Domain.ConciliacaoContabil.Sagas.Messages;

[Endpoint("Korp.Contabilidade.ConciliacaoContabil.ConciliarApuracaoLancamento")]
public class ConciliarApuracaoLancamentoCommand: ICommand
{
    public Guid IdConciliacaoContabil { get; set; }   
}