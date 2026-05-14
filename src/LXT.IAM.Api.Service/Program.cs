using LXT.IAM.Api.Bll.Mapper;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Common.Intefaces;
using LXT.IAM.Api.Common.Orms.Dapper;
using LXT.IAM.Api.Common.Utils;
using LXT.IAM.Api.Dal.Dals.Base;
using LXT.IAM.Api.Model.Options;
using LXT.IAM.Api.Service.Extensions;
using LXT.IAM.Api.Service.Filters;
using LXT.IAM.Api.Service.Middlewares;
using LXT.IAM.Api.Storage.Context;
using Maomi.I18n;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpContextUtility, HttpContextUtility>();
builder.Services.AddScoped<IDapperProvider, DapperProvider>();
builder.Services.AddScoped<JwtTokenHelper>();
builder.Services.Configure<SmsOptions>(builder.Configuration.GetSection("SMS"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("AppOptions"));
builder.Services.Configure<DouyinAppOptions>(builder.Configuration.GetSection("DouyinAppOptions"));

builder.Services.AddI18nAspNetCore(defaultLanguage: "zh-CN");
builder.Services.AddI18nResource(options =>
{
    var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "I18n").ToString();
    if (Directory.Exists(basePath))
    {
        options.AddJsonDirectory(basePath);
    }
});

builder.Services.AddDbContext<IAMDbContext>(options =>
{
    options.UseMySql(builder.Configuration["Db:IAMDb:ConnStr"], new MySqlServerVersion(new Version(8, 0, 27)))
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});

builder.Services.AddScoped(typeof(IBaseDal<>), typeof(BaseDal<>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "IAM API文档" });
    Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file => options.IncludeXmlComments(file, true));
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "请输入带有Bearer的Token，形如“Bearer {Token}”",
        Name = HttpHeaderConst.Authorization,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<AcceptLanguageHeaderFilter>();
});

Log.Logger = ServiceCollectionExtension.GetLogConfig("LXT.IAM.Api", bool.Parse(builder.Configuration["Loki:Enable"] ?? "false"), builder.Configuration["Loki:Url"]);
builder.Host.UseSerilog();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5),
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecurityKey"]!))
    };
});

builder.Services.AddBussinessObjectInjection();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
builder.Services.AddSnowflakeId(ushort.Parse(builder.Configuration["SnowflakeIdOptions:WorkId"] ?? "1"));
builder.Services.AddTransient<IStartupFilter, MigrateStartupFilter>();

var app = builder.Build();

if (bool.TryParse(app.Configuration["Swagger:Enable"], out var isEnable) && isEnable)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseI18n();
app.UseStaticFiles();
app.UseCors("default");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
