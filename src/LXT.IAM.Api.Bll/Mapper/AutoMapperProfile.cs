using AutoMapper;
using LXT.IAM.Api.Common.Models;

namespace LXT.IAM.Api.Bll.Mapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap(typeof(PagedList<>), typeof(PagedList<>));
    }
}
