using System;
using Viasoft.Accounting.Domain.Enums;

namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public interface IHaveBorrower
    {
        CteBorrower Borrower { get; set; }
           
        Guid? Shipper { get; set; }
       
        Guid? Addressee { get; set; }
       
        Guid? Dispatcher { get; set; }
       
        Guid? Receiver { get; set; }
       
        Guid? OtherBorrower { get; set; }
    }
}