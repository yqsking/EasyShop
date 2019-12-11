using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using EasyShop.Appliction.DataTransferModels.User;
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
        /// 根据唯一Id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserResponseDto> GetUser(string id)
        {
            var model = await  _userRepository.GetEntityAsync(id.Trim());
            return _mapper.Map<UserResponseDto>(model);
        }

        /// <summary>
        /// 根据搜索条件获取单个用户
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
        /// 根据搜索条件分页获取用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<PageResult<UserResponseDto>> GetUserPageList(GetUserPageListRequestDto dto)
        {
            Expression<Func<Dommain.Entitys.User.UserEntity, bool>> expression = item => true;
            expression= expression.AndIf(!string.IsNullOrWhiteSpace(dto.UserName), item => item.UserName.Contains(dto.UserName.Trim()))
                      .AndIf(!string.IsNullOrWhiteSpace(dto.Phone),item=>item.Phone.Contains(dto.Phone.Trim()))
                      .AndIf(!string.IsNullOrWhiteSpace(dto.QQNumber),item=>item.QQNumber.Contains(dto.QQNumber.Trim()))
                      .AndIf(!string.IsNullOrWhiteSpace(dto.WeCharNumber),item=>item.WeCharNumber .Contains(dto.WeCharNumber.Trim()));
            var result = await _userRepository.GetEntityPageList(dto.PageIndex,dto.PageSize ,expression,item=>item.CreateTime);
            return new PageResult<UserResponseDto>
            {
                PageIndex = result.PageIndex,
                PageSize = result.PageSize,
                TotalNumber = result.TotalNumber,
                TotalPageIndex = result.TotalPageIndex,
                Data =_mapper.Map<List<UserResponseDto>>(result.Data)
            };
        }


    }
}
