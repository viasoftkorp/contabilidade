using System;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Host.Messages
{
    [Endpoint("Viasoft.Billing.CTE.CteCanceled")]
    public class CteCanceled : IMessage
    {
        public Guid CteId { get; set; }
        public string Reason { get; set; }
    }
}