using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EasyShop.Api.Controllers
{
    /// <summary>
    /// 公共模块
    /// </summary>
    [ApiController]
    [Route("api/commons")]
    public class CommonController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        public CommonController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("uploadImages")]
        public async Task<IActionResult> UploadImages(IFormFile file)
        {
            var url = Request.Scheme + "://" + Request.Host;
            if (file == null)
            {
                file = Request.Form.Files.FirstOrDefault();
            }
            if (file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var path = "/ResourcesFile";
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
                    return new JsonResult(url + @"/src/Images/" + fileName);
                }
            }
            return new JsonResult("");
        }

        /// <summary>
        /// 下载文件接口
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpGet("downloadImages")]
        public async Task<IActionResult> DownloadFileAsync(string filePath)
        {
            var addrUrl = _webHostEnvironment.ContentRootPath + filePath;
            var stream = System.IO.File.OpenRead(addrUrl);
            var result = await Task.Run(() => File(stream, "application/octet-stream", Path.GetFileName(addrUrl)));
            return result;
        }
    }
}
