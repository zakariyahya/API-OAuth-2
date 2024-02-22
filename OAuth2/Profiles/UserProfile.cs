using AutoMapper;
using OAuth2.Models.User;
using OAuth2.Models.User.Requests;

namespace OAuth2.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<UserCreateRequest, UserModel>();
        }
    }
}
