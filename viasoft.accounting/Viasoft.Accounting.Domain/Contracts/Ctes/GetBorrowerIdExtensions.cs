using System;
using Viasoft.Accounting.Domain.Enums;

namespace Viasoft.Accounting.Domain.Contracts.Ctes
{
    public static class GetBorrowerIdExtensions
    {
        public static Guid? GetBorrowerId(this IHaveBorrower borrower)
        {
            switch (borrower.Borrower)
            {
                case CteBorrower.Addressee:
                    return borrower.Addressee.Value;      
                
                case CteBorrower.Dispatcher:
                    return borrower.Dispatcher.Value;               
                case CteBorrower.Shipper:
                    return borrower.Shipper.Value;               
                case CteBorrower.Other:
                    return borrower.OtherBorrower.Value;                
                case CteBorrower.Receiver:
                    return borrower.Receiver.Value;
                default:
                    throw new Exception("Not registered borrower");
            }
        }
    }
}