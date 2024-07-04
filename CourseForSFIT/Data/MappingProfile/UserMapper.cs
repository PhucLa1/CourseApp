using AutoMapper;
using Data.Entities;
using Dtos.Models;


namespace Data.Mapping
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserSignUpDto, User>()
                .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
            CreateMap<FacebookUserInfoResult, User>()
               .ForMember(destination => destination.Avatar, opt => opt.MapFrom(src => src.FacebookPicture.Data.GetFileNameFromUrl()))
               .ForAllMembers(opt => opt.Condition((src, destination, srcMember) => srcMember != null));
        }
    }
}
