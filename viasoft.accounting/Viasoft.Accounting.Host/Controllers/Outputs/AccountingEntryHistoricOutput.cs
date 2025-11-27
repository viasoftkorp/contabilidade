using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Controllers.Outputs
{
    public class AccountingEntryHistoricOutput : FullAuditedEntityDto
    {
        public string Code { get; set; }        
        public string Description { get; set; }
        
        public bool IsStatic { get; set; }
        
        public string Module { get; set; }

    }
}