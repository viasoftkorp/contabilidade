using AutoMapper;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Outputs;

namespace Viasoft.Accounting.Host.Mappers
{
    public class AccountingOperationMapper : Profile
    {
        public AccountingOperationMapper()
        {
            CreateAccountOperationMapping();
        }

        private void CreateAccountOperationMapping()
        {
            CreateMap<AccountingOperation, AccountingOperationOutput>();
        }
    }
}