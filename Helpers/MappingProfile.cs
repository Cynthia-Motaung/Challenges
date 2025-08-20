using AutoMapper;
using Challenges.Models;
using Challenges.Models.DTOs;


public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        // User to DTO mappings
        CreateMap<User, UserIndexDto>();
        CreateMap<User, UserDetailsDto>();
        CreateMap<User, UserEditDto>(); // Maps User entity to UserEditDto for displaying existing data

        // DTO to User mappings
        CreateMap<UserCreateDto, User>(); // Maps UserCreateDto to a new User entity
        // For updates, use .ForMember(dest => dest.Id, opt => opt.Ignore()); 
        // to prevent AutoMapper from trying to map the Id when updating an existing entity.
        // Also, ignore CreatedAt as it should not be updated from DTO
        CreateMap<UserEditDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
    }
}
