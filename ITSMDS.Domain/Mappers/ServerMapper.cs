
using AutoMapper;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;

namespace ITSMDS.Domain.Mappers;

public class ServerMapper : Profile
{
    public ServerMapper()
    {
        CreateMap<ServerEntity, ServerDto>()
            .ReverseMap();
    }
}
