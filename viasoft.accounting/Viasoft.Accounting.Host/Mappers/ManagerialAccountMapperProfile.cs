using AutoMapper;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Outputs;

namespace Viasoft.Accounting.Host.Mappers
{
    public class ManagerialAccountMapperProfile : Profile
    {
        public ManagerialAccountMapperProfile()
        {
            MapManagerialAccount();
        }

        private void MapManagerialAccount()
        {
            CreateMap<ManagerialAccount, ManagerialAccountOutput>();
        }

    }
}