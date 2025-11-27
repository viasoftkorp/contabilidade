using AutoMapper;
using Viasoft.Accounting.Domain.Entities;
using Viasoft.Accounting.Host.Controllers.Outputs;

namespace Viasoft.Accounting.Host.Mappers
{
    public class BookkeepingMapperProfile : Profile
    {
        public BookkeepingMapperProfile()
        {
            MapBookkeeping();
        }

        private void MapBookkeeping()
        {
            CreateMap<BookkeepingAccount, BookkeepingAccountOutput>();
        }

    }
}
