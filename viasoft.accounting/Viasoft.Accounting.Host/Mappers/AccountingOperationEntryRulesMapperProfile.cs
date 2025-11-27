using AutoMapper;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Inputs;
using Viasoft.Accounting.Host.Controllers.Outputs;

namespace Viasoft.Accounting.Host.Mappers
{
    public class AccountingOperationEntryRulesMapperProfile : Profile
    {
        public AccountingOperationEntryRulesMapperProfile()
        {
            MapEntryRules();
        }

        private void MapEntryRules()
        {
            CreateMap<AccountingOperationEntryRule, AccountingOperationEntryRuleOutput>();
            CreateMap<AccountingOperationEntryRuleInput, AccountingOperationEntryRule>();
        }
    }
}