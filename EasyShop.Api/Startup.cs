using AutoMapper;
using EasyShop.Api.MiddleWare;
using EasyShop.Appliction.AutoMapper;
using EasyShop.BasicImpl.DBContext;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace EasyShop.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //设置枚举格式
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            //配置允许跨域访问 (PS:同时配置 AllowCredentials 和 AllowAnyOrigin 会冲突报错)
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddHttpClient();

            //依赖注入automapper
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            //依赖注入中介者
            services.AddMediatR(typeof(IMediator));

            //依赖注入公用服务
            services.RegisterCommon();
            //依赖注入仓储
            services.RegisterRepositorys();
            //依赖注入命令
            services.RegisterCommand();
            //依赖注入查询器
            services.RegisterQueries();

            //依赖注入数据库上下文对象
            var sqlConnection = Configuration.GetConnectionString("Default");
            services.AddDbContext<EasyShopDBContext>(option => option.UseSqlServer(sqlConnection, provider0ptions => provider0ptions.CommandTimeout(120)));

            //添加swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "EasyShop API",
                    Version = "v1"
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPathApi = Path.Combine(basePath, "EasyShop.Api.xml");
                var xmlPathAppliction = Path.Combine(basePath, "EasyShop.Appliction.xml");
                options.IncludeXmlComments(xmlPathApi);
                options.IncludeXmlComments(xmlPathAppliction);

            });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            var path = Configuration.GetValue<string>("ResourcesPath");
            //指定允许访问的静态资源根目录
            app.UseStaticFiles(
                new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),path)),
                    RequestPath = new PathString($"/{path}")
                } );
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            app.UseAuthorization();
            //启动Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyShop API V1");
                c.OAuthClientId("EasyShopSwaggerUI");
                c.OAuthAppName("EasyShop Swagger UI");
            });
            //启动允许跨域访问
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            // 短路中间件，配置Controller路由
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
