using AutoMapper;
using EasyShop.Api.Filters;
using EasyShop.Api.JWT.Requirements;
using EasyShop.Api.MiddleWare;
using EasyShop.Appliction.AutoMapper;
using EasyShop.BasicImpl.DBContext;
using EasyShop.CommonFramework.Exception;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 依赖注入服务
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
            })
            .ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; });//启用dto模型验证
            //配置允许跨域访问 (PS:同时配置 AllowCredentials 和 AllowAnyOrigin 会冲突报错)
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddHttpClient();
            //自定义jwt授权策略
            services.AddAuthorization(
                options=>
                { 
                    options.AddPolicy("jwtRequirement", policy => policy.Requirements.Add(new JwtAuthorizationRequirement())); }
                ).AddAuthentication(option=>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(option=>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Authentication:JwtBearer:Issuer"],//检查令牌的颁发者
                        ValidAudience = Configuration["Authentication:JwtBearer:Audience"],//检查令牌的有效访问者
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:JwtBearer:SecurityKey"])),
                        ClockSkew = TimeSpan.Zero,
                        ValidateLifetime = true
                    };
                    option.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            //Token expired
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        //此处为权限验证失败后触发的事件
                        OnChallenge = context =>
                        {
                            //此处代码为终止.Net Core默认的返回类型和数据结果
                            context.HandleResponse();
                            throw new CustomException("抱歉，当前账户登录授权失败！");
                        }
                    };
                });

            //依赖注入automapper
            services.AddAutoMapper(typeof(UserProfile).Assembly);

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
                //添加JWT授权
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPathApi = Path.Combine(basePath, "EasyShop.Api.xml");
                var xmlPathAppliction = Path.Combine(basePath, "EasyShop.Appliction.xml");
                options.IncludeXmlComments(xmlPathApi);
                options.IncludeXmlComments(xmlPathAppliction);
                options.SchemaFilter<SwaggerExcludeFilter>();

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
            else
            {
                app.UseHsts();
            }
            app.UseRouting();
            //指定允许访问的静态资源根目录
            var path = Configuration.GetValue<string>("ResourcesPath");
            app.UseStaticFiles(
                new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),path)),
                    RequestPath = new PathString($"/{path}")
                } );
            //全局异常处理
            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            app.UseAuthorization();
            app.UseAuthentication();//认证中间件
           
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
