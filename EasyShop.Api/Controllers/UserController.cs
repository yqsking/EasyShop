﻿using System.Net;
using System.Threading.Tasks;
using EasyShop.Appliction.Commands.User;
using EasyShop.Appliction.Queries;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace EasyShop.Api.Controllers
{
    /// <summary>
    /// 用户模块
    /// </summary>
    [ApiController]
    [Route("api/users")]
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
    }
}
