
using AutoMapper;
using ITSMDS.Domain.DTOs;
using ITSMDS.Domain.Entities;
using ITSMDS.Domain.Tools;

namespace ITSMDS.Domain.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UpdateUserRequest>()
            .ForMember(d => d.Email, m => m.MapFrom(s => s.Email)).ReverseMap();
        CreateMap<User, UserResponse>()
            .ForMember(d => d.createDate, m => m.MapFrom(m => ConvertDate.ConvertToShamsi(m.CreateDate)));
        CreateMap<User, UserResponse>()
          .ConstructUsing(src => new UserResponse(
              src.HashId,
              src.Email,
              src.FirstName,
              src.LastName,
              ConvertDate.ConvertToShamsi(src.CreateDate),
              src.PhoneNumber,
              src.IpAddress,
              src.UserName,
              src.PersonalCode
              ))
          .ReverseMap();
        CreateMap<User, UpdateUserRequest>()
          .ReverseMap();
    }
}
