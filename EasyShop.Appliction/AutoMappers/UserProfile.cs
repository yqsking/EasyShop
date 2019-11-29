using AutoMapper;

namespace EasyShop.Appliction.AutoMapper
{
    /// <summary>
    /// 
    /// </summary>
    public  class UserProfile:Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public UserProfile()
        {
            CreateMap<Dommain.Entitys.User.UserEntity,ViewModels.User.UserResponseDto>();
            CreateMap<Dommain.Entitys.User.UserEntity, ViewModels.User.ExportUserResponseDto>();
        }
    }
}
