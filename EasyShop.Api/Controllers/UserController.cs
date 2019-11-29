using System;
using System.Net;
using System.Threading.Tasks;
using EasyShop.Api.Filters;
using EasyShop.Appliction.Commands.User;
using EasyShop.Appliction.DataTransferModels.User;
using EasyShop.Appliction.Queries;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using EasyShop.CommonFramework.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;


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
        /// 导出符合条件的用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("exprot")]
        public async Task<IActionResult> GetExportUserList([FromQuery]GetExportUserListRequestDto dto)
        {
            var result = await _userQueries.GetExportUserList(dto);
            byte[] buff = ExcelHelper.Export(result);
            Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            return File(buff, "application/vnd.ms-excel", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");

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
