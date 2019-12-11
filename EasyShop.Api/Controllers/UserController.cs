using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EasyShop.Api.Filters;
using EasyShop.Appliction.Commands.User;
using EasyShop.Appliction.DataTransferModels.User;
using EasyShop.Appliction.Queries;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EasyShop.Api.Controllers
{
    /// <summary>
    /// 用户模块
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [TypeFilter(typeof(VerificationFilter))]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserQueries _userQueries;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="userQueries"></param>
        public UserController(IMediator mediator, IUserQueries userQueries)
        {
            _mediator = mediator;
            _userQueries = userQueries;
        }

      
        /// <summary>
        /// 获取指定用户信息
        /// </summary>
        /// <param name="id">用户唯一id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserResponseDto))]
        public async Task<IActionResult> GetUser([FromRoute]string id)
        {
           var result=await  _userQueries.GetUser(id);
           return Ok(result);
          
        }

        /// <summary>
        /// 分页获取所有用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PageResult<UserResponseDto>))]
        public async Task<IActionResult> GetUserPageList([FromQuery]GetUserPageListRequestDto dto)
        {
            var result = await _userQueries.GetUserPageList(dto);
            return Ok(result);
        }

        /// <summary>
        /// 根据手机号码登录获取Token
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("phoneToken")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResult<string>))]
        public async Task<IActionResult> GetTokenByPhone([FromBody]GetTokenByPhoneRequestDto dto)
        {
            //从数据库验证用户名，密码 
            var result = await _userQueries.GetTokenByPhone(dto);
            return Ok(result);
           
        }


        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResult<UserResponseDto>))]
        public async Task<IActionResult> UserRegister([FromBody]RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// 更新用户基本资料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResult<UserResponseDto>))]
        public async Task<IActionResult> UpdateUser([FromRoute]string id,[FromBody]UpdateUserCommand command)
        {
            command.SetId(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

      
    }
}
