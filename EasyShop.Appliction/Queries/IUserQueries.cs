using EasyShop.Appliction.DataTransferModels.User;
using EasyShop.Appliction.ViewModels;
using EasyShop.Appliction.ViewModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyShop.Appliction.Queries
{
    /// <summary>
    /// 用户模块查询器
    /// </summary>
    public  interface IUserQueries
    {
        /// <summary>
        /// 根据唯一Id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserResponseDto> GetUser(string id);
        /// <summary>
        /// 根据搜索条件获取单个用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UserResponseDto> GetUser(GetUserRequestDto dto);
        /// <summary>
        /// 根据搜索条件分页获取用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PageResult<UserResponseDto>> GetUserPageList(GetUserPageListRequestDto dto);

        /// <summary>
        /// 获取所有符合条件的导出用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<List<ExportUserResponseDto>> GetExportUserList(GetExportUserListRequestDto dto);
    }
}
