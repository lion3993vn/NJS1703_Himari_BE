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
            services.Configure<RedisSettings>(config.GetSection("RedisSettings"));
            services.AddScoped<IRedisService, RedisService>();

            // config user service
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            // config category service
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();    
            
            // config product service
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            // config blog service
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IBlogService, BlogService>();

            // config bodypart service
            services.AddScoped<IBodyPartRepository, BodyPartRepository>();
            services.AddScoped<IBodyPartService, BodyPartService>();            
            
            // config brand service
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();

            // config symptom service
            services.AddScoped<ISymptomRepository, SymptomRepository>();
            services.AddScoped<ISymptomService, SymptomService>();
            return services;
        }
    }
}
