using AutoMapper;
using Ryder.Application.Rider.Query.AllRiderHistory;
using Ryder.Domain.Entities;

namespace Ryder.Application.Common.Mapper
{
    public class AutoMapperInitializer : Profile
    {
        public AutoMapperInitializer()
        {
            CreateMap<Address, Location>();
            CreateMap<Domain.Entities.Order, GetOrderResponse>();
        }
    }
}