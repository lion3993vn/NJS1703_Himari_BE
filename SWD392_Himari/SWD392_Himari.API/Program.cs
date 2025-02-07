using Backend_reactNative_Shoppee_Data.Middleware;
using Backend_reactNative_Shoppee_Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWD392_Himari.Repository;
using SWD392_Himari.Repository.Data;
using SWD392_Himari.Repository.Mapping;
using SWD392_Himari.Repository.Middleware;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Add repositories and services
        builder.Services
                .AddRepository() // Assuming AddRepository is an extension method to add repositories
                .AddServices();   // Assuming AddServices is an extension method to add your services


        // Add CursusDbContext with SQL Server configuration
        builder.Services.AddDbContext<SWD392HimariDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HimariDB"),
        sqlOptions =>
        {
            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            sqlOptions.CommandTimeout(30);
        }));


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = "Cookies"; // Use cookies for sign-in
        })
        .AddCookie("Cookies") // Add cookie-based authentication

        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Cookies["authToken"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
        //.AddFacebook(facebookOptions =>
        //{
        //    facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        //    facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
        //    facebookOptions.CallbackPath = "/signin-facebook";
        //    facebookOptions.Scope.Add("user_birthday");     // Quyền truy cập hình ảnh
        //    facebookOptions.Scope.Add("age_range");     // Quyền truy cập hình ảnh
        //    facebookOptions.Scope.Add("user_hometown");     // Quyền truy cập hình ảnh

        //});





        // Đăng ký middleware xử lý lỗi của bạn

        // Add AutoMapper for mapping between objects
        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        // add cors
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                // Allow access from mobile devices on local network
                policy.WithOrigins(
                          "http://localhost:8081",
                          "https://192.168.100.8:8081",
                          "https://192.168.100.8:5000"
                      ).AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();  // Allow credentials like cookies, tokens, etc.
            });
        });




        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Configure JSON serialization to handle reference cycles
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SWD392_Himari", Version = "v1.0" });

            // Cấu hình Bearer Authentication
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "The 'Bearer' prefix will be automatically added.",

            });

            // Cấu hình yêu cầu bảo mật
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
           
        });

        builder.Services.AddMemoryCache();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        // Cấu hình SSL để sử dụng HTTPS

        //middleware
        app.UseSwagger();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SWD392_Himari v1.0");
                c.DefaultModelsExpandDepth(-1); // Ẩn phần Models
            });
        }
        if (!app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
                options.DefaultModelsExpandDepth(-1);
            });
        }
        app.UseCors("AllowSpecificOrigins");

        //app.UseSession();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<DelayMiddleware>();
        app.UseMiddleware<SuccessMiddleware>();

        app.UseAuthentication();
        app.UseAuthorization();
        // end middleware

        app.MapControllers();

        app.Run();
    }
}