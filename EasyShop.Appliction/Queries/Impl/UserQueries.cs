using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EasyShop.Appliction.DataTransferModel.User;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using EasyShop.Dommain.Repositorys.User;

namespace EasyShop.Appliction.Queries.Impl
{
    /// <summary>
    /// 用户模块查询器
    /// </summary>
    public class UserQueries : IUserQueries
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public UserQueries(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserResponseDto> GetUser(string id)
        {
            var model = await  _userRepository.GetEntityAsync(id.Trim());
            return _mapper.Map<UserResponseDto>(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UserResponseDto> GetUser(GetUserRequestDto dto)
        {
            Expression<Func<Dommain.Entitys.User.UserEntity, bool>> expression = item => true;
            var model = await _userRepository.GetEntityAsync(expression);
            return _mapper.Map<UserResponseDto>(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PageResult<UserResponseDto>> GetUserPageList(GetUserPageListRequestDto dto)
        {
            Expression<Func<Dommain.Entitys.User.UserEntity, bool>> expression = item => true;
            if(!string.IsNullOrWhiteSpace(dto.UserName))
            {
                expression = expression.And(item=>item.UserName.Contains(dto.UserName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(dto.Phone))
            {
                expression = expression.And(item => item.Phone.Contains(dto.Phone.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(dto.QQNumber))
            {
                expression = expression.And(item => item.QQNumber.Contains(dto.QQNumber.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(dto.WeCharNumber))
            {
                expression = expression.And(item => item.WeCharNumber.Contains(dto.WeCharNumber.Trim()));
            }
            var list = await _userRepository.GetEntityPageList(dto.PageIndex,dto.PageSize ,expression,item=>item.CreateTime);
            throw new NotImplementedException();
        }

    }
}
