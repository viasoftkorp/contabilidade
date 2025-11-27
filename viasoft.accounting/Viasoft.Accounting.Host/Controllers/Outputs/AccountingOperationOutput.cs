using Viasoft.Accounting.Domain.Entities;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Controllers.Outputs
{
    public class AccountingOperationOutput: FullAuditedEntityDto
    {
        public AccountingOperationOutput()
        {

        }
        public AccountingOperationOutput(AccountingOperation accountingOperation)
        {
            Code = accountingOperation.Code;
            Description = accountingOperation.Description;
            Cfop = accountingOperation.Cfop;
            EvaluatedSocialContribution = accountingOperation.EvaluatedSocialContribution;
            DoesntGenerateUnitaryCost = accountingOperation.DoesntGenerateUnitaryCost;
            ShouldGenerateEntries = accountingOperation.ShouldGenerateEntries;
            CstIcms = accountingOperation.CstIcms;
            CstPis = accountingOperation.CstPis;
            CstCofins = accountingOperation.CstCofins;
            CteModule = accountingOperation.CteModule;
            Id = accountingOperation.Id;
            IssueInvoice = accountingOperation.IssueInvoice ?? false;
            InvoiceEntryOperation = accountingOperation.InvoiceEntryOperation ?? false;
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public string Cfop { get; set; }
        public string EvaluatedSocialContribution { get; set; }
        public string DoesntGenerateUnitaryCost { get; set; }
        public string ShouldGenerateEntries { get; set; }
        public string CstIcms { get; set; }
        public string CstPis { get; set; }
        public string CstCofins { get; set; }
        public bool CteModule { get; set; }
        public bool IssueInvoice { get; set; }
        public bool InvoiceEntryOperation { get; set; }
    }
}
