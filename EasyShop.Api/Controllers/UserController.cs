using System.Net;
using System.Threading.Tasks;
using EasyShop.Api.Filters;
using EasyShop.Appliction.Commands.User;
using EasyShop.Appliction.DataTransferModels.User;
using EasyShop.Appliction.Queries;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyShop.Api.Controllers
{
    /// <summary>
    /// 用户模块
    /// </summary>
    //[Authorize(Policy = "jwtRequirement")]
    [ApiController]
    [Route("api/user")]
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
        [Route("allUser")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PageResult<UserResponseDto>))]
        public async Task<IActionResult> GetUserPageList([FromQuery]GetUserPageListRequestDto dto)
        {
            var result = await _userQueries.GetUserPageList(dto);
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
