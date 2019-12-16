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
        /// �����ļ�
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// ����ע�����
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
            })
            .ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; });//����dtoģ����֤
            //�������������� (PS:ͬʱ���� AllowCredentials �� AllowAnyOrigin ���ͻ����)
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddHttpClient();
            //�Զ���jwt��Ȩ����
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
                        ValidIssuer = Configuration["Authentication:JwtBearer:Issuer"],//������Ƶİ䷢��
                        ValidAudience = Configuration["Authentication:JwtBearer:Audience"],//������Ƶ���Ч������
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
                        //�˴�ΪȨ����֤ʧ�ܺ󴥷����¼�
                        OnChallenge = context =>
                        {
                            //�˴�����Ϊ��ֹ.Net CoreĬ�ϵķ������ͺ����ݽ��
                            context.HandleResponse();
                            throw new CustomException("��Ǹ����ǰ�˻���¼��Ȩʧ�ܣ�");
                        }
                    };
                });

            //����ע��automapper
            services.AddAutoMapper(typeof(UserProfile).Assembly);

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
                //���JWT��Ȩ
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "���¿�����������ͷ����Ҫ���Jwt��ȨToken��Bearer Token",
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
            //ָ��������ʵľ�̬��Դ��Ŀ¼
            var path = Configuration.GetValue<string>("ResourcesPath");
            app.UseStaticFiles(
                new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),path)),
                    RequestPath = new PathString($"/{path}")
                } );
            //ȫ���쳣����
            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            app.UseAuthorization();
            app.UseAuthentication();//��֤�м��
           
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
