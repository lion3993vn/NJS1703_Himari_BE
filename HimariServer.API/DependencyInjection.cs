using HimariServer.Repository.Repositories.Implements;
using HimariServer.Repository.Repositories.Interfaces;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.Services.Implements;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;

namespace HimariServer.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfractstructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // config redis service
            //services.Configure<RedisSettings>(config.GetSection("RedisSettings"));
            //services.AddScoped<IRedisService, RedisService>();

            // config user service
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
