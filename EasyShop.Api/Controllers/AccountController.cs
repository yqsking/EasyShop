﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EasyShop.Appliction.Queries;
using System.Net;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.DataTransferModels.User;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using EasyShop.CommonFramework.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EasyShop.Api.Controllers
{
    /// <summary>
    /// 授权模块
    /// </summary>
    [AllowAnonymous]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IUserQueries _userQueries;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userQueries"></param>
        /// <param name="configuration"></param>
        public AccountController(IUserQueries userQueries, IConfiguration configuration)
        {
            _userQueries = userQueries;
            _configuration = configuration;
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
            if (result.IsSuccess)
            {
                var userClaims = new[] {
                        new Claim("UserId",result.Data.Id),
                        new Claim("UserType",result.Data.UserType),
                    };
                string token = JwtHelper.GetToken(_configuration, userClaims);
                return Ok(new ApiResult<string>() { IsSuccess = true, Message = "登录成功！", Data = token });
            }
            else
            {
                return Ok(new ApiResult<string>() { IsSuccess = false, Message = result.Message, Data = default });
            }


        }


    }
}
