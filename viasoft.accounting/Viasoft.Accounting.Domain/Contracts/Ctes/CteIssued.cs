using System;
using System.Collections.Generic;
using Viasoft.Accounting.Domain.Enums;
using Viasoft.Core.ServiceBus.Abstractions;

namespace Viasoft.Accounting.Domain.Contracts.Ctes
{

    [Endpoint("Viasoft.Billing.CTE.CteIssued")]
    public class CteIssued : IMessage, IHaveBorrower
    {
        public Guid Id { get; set; }

        public Guid AccountingOperationId { get; set; }
        public int SeriesNumber { get; set; }
        public int? CteNumber { get; set; }
        public DateTime EmissionDate { get; set; }

        public Guid? Shipper { get; set; }

        public Guid? Addressee { get; set; }

        public Guid? Dispatcher { get; set; }

        public Guid? Receiver { get; set; }

        public Guid? OtherBorrower { get; set; }
        
        public string BorrowerCode { get; set; }

        public CteBorrower Borrower { get; set; }
        public Dictionary<CteComplement, string> Complements { get; set; }
        public Dictionary<string, decimal> Variables { get; set; }

    }
}