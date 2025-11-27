using Viasoft.Accounting.Domain.Entities;
using Viasoft.Core.DDD.Application.Dto.Entities;

namespace Viasoft.Accounting.Host.Controllers.Outputs
{
    public class BookkeepingAccountOutput : EntityDto
    {
        public BookkeepingAccountOutput()
        {
            
        }
        public BookkeepingAccountOutput(BookkeepingAccount conta)
        {
            Id = conta.Id;
            Code = conta.Code;
            Classification = conta.Classification;
            Name = conta.Name;
        }

        public int Code { get; set; }
        
        public string Classification { get; set; }
        
        public string Name { get; set; }
    }
}