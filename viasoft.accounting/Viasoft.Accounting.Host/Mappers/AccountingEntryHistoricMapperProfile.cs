using AutoMapper;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Outputs;

namespace Viasoft.Accounting.Host.Mappers
{
    public class AccountingEntryHistoricMapperProfile : Profile
    {
        public AccountingEntryHistoricMapperProfile()
        {
            MapEntryHistoric();
        }

        private void MapEntryHistoric()
        {
            CreateMap<AccountingEntryHistory, AccountingEntryHistoricOutput>();
        }
    }
}