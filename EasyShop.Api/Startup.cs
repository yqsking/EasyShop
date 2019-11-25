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
                //����ѭ������
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //����ö�ٸ�ʽ
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                //����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });
            //�������������� (PS:ͬʱ���� AllowCredentials �� AllowAnyOrigin ���ͻ����)
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddHttpClient();

            //����ע��automapper
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            //����ע���н���
            services.AddMediatR(typeof(IMediator));

            //����ע�빫�÷���
            services.RegisterCommon();
            //����ע��ִ�
            services.RegisterRepositorys();
            //����ע������
            services.RegisterCommand();
            //����ע���ѯ��
            services.RegisterQueries();

            //����ע�����ݿ������Ķ���
            var sqlConnection = Configuration.GetConnectionString("Default");
            services.AddDbContext<EasyShopDBContext>(option => option.UseSqlServer(sqlConnection, provider0ptions => provider0ptions.CommandTimeout(120)));

            //���swagger
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
            //ָ��������ʵľ�̬��Դ��Ŀ¼
            app.UseStaticFiles(
                new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),path)),
                    RequestPath = new PathString($"/{path}")
                } );
            app.UseMiddleware<ExceptionHandlerMiddleWare>();
            app.UseAuthorization();
            //����Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EasyShop API V1");
                c.OAuthClientId("EasyShopSwaggerUI");
                c.OAuthAppName("EasyShop Swagger UI");
            });
            //��������������
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            // ��·�м��������Controller·��
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
