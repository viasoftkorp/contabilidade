using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Controllers.Outputs
{
    public class TaxesOutput : EntityDto
    {
        public int Code { get; set; }
        
        public string Description { get; set; }
    }
}