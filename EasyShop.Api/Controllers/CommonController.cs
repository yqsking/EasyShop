using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EasyShop.Api.Filters;
using EasyShop.Appliction.ViewModels;
using EasyShop.CommonFramework.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EasyShop.Api.Controllers
{
    /// <summary>
    /// 公共模块
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("api/common")]
    public class CommonController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <param name="configuration"></param>
        public CommonController(IWebHostEnvironment webHostEnvironment,IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("uploadImages")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResult<string>))]
        public async Task<IActionResult> UploadImages(IFormFile file)
        {
            ApiResult<string> result = new ApiResult<string>();
            var url = Request.Scheme + "://" + Request.Host;
            if (file == null)
            {
                file = Request.Form.Files.FirstOrDefault();
            }
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                var resourcesPath = _configuration.GetValue<string>("ResourcesPath");
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, resourcesPath);
                if (!Directory.Exists(path))//如果不存在就创建文件夹
                {
                    Directory.CreateDirectory(path);//创建该文件夹　
                }
                path = Path.Combine(path, fileName);
                using (var stream = new FileStream(path, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                    stream.Flush();
                    stream.Dispose();
                    string fileUrl = Path.Combine(Path.Combine(url, resourcesPath), fileName).Replace("\\","/");
                    result.IsSuccess = true;
                    result.Message = "上传文件成功！";
                    result.Data = fileUrl;
                    return Ok(result);
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "抱歉，服务端没有收到任何文件！";
                return Ok(result);
            }
           

        }

        /// <summary>
        /// 下载文件接口
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("downloadImages")]
        public async Task<IActionResult> DownloadFileAsync(string filePath)
        {
            string url = Request.Scheme + "://" + Request.Host;
            if (filePath.Contains(url))
            {
                filePath = filePath.Replace(url+"/","");
            }
            if(!System.IO.File.Exists(filePath))
            {
                throw new CustomException("抱歉，当前文件路径不存在！");
            }
            var stream = System.IO.File.OpenRead(filePath);
            var result = await Task.Run(() => File(stream, "application/octet-stream", Path.GetFileName(filePath)));
            return result;
        }
    }
}
