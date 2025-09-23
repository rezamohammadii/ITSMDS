
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
            .ForMember(d => d.Email, m => m.MapFrom(s => s.Email))
            .ReverseMap();

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
                src.PersonalCode,
                src.UserRoles != null ?
                    src.UserRoles.Select(ur => ur.Role.Name ?? "").Where(name => !string.IsNullOrEmpty(name)).ToList()
                    : new List<string>(), src.IsActive // اضافه کردن roleName
            ))
            .ForMember(d => d.createDate, m => m.MapFrom(m => ConvertDate.ConvertToShamsi(m.CreateDate)))
            .ReverseMap();

        CreateMap<User, UpdateUserRequest>()
            .ReverseMap();
    }
}
