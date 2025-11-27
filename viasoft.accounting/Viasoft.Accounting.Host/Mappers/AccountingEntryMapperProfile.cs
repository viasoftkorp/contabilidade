using Viasoft.Accounting.Domain.Dtos;
using Viasoft.Accounting.Domain.Entities;
using AutoMapper;
using Viasoft.Accounting.Host.Contracts;


namespace Viasoft.Accounting.Host.Mappers
{
    public class AccountingEntryMapperProfile : Profile
    {


        public AccountingEntryMapperProfile()
        {
            CreateAccountingEntryMaps();
        }
        
        private void CreateAccountingEntryMaps()
        {
            CreateMap<AccountingEntryDto, AccountingEntry>();
            CreateMap<AccountingEntryItemDto, AccountingEntryItem>();
        }
    }
}