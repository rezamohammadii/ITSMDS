
using AutoMapper;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using ITSMDS.Domain.Tools;

namespace ITSMDS.Domain.Mappers;

public class ServerMapper : Profile
{
    public ServerMapper()
    {

        CreateMap<ServerEntity, ServerDto>()
         .ForMember(d => d.CreateDate, m => m.MapFrom(s => ConvertDate.ConvertToShamsi(s.StartDate)))
           .ForMember(dest => dest.Services,
                      opt => opt.MapFrom(src => src.Services))
         .ReverseMap();
    }
}
