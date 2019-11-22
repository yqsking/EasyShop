using AutoMapper;

namespace EasyShop.Appliction.AutoMapper
{
    public  class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<Dommain.Entitys.User.UserEntity,ViewModels.User.UserResponseDto>();
        }
    }
}
