

using AutoMapper;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using ITSMDS.Domain.Tools;

namespace ITSMDS.Domain.Mappers;

public class ServiceMapper : Profile
{
    public ServiceMapper()
    {
        CreateMap<ServiceEntity, ServiceDto>()
            .ReverseMap();

        CreateMap<ServiceEntity, ServiceDto>()
         .ForMember(d => d.CreateTime, m => m.MapFrom(s => ConvertDate.ConvertToShamsi(s.CreateTime)))
         .ForMember(d => d.ServerName, m => m.MapFrom(s => s.Name))
         .ReverseMap();
    }
}
