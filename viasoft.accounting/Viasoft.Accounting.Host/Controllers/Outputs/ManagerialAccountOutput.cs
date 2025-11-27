using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Controllers.Outputs
{
    public class ManagerialAccountOutput : EntityDto
    {
        public int Code { get; set; }
            
        public string Description { get; set; }
    }
}