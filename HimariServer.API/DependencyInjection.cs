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
            
            //config blogCategory services

            services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
            services.AddScoped<IBlogCategoryService,BlogCategoryService>();

            // config brand service
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();

            // config user device service
            services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
            services.AddScoped<IUserDeviceService, UserDeviceService>();

            // config role service
            services.AddScoped<IRoleRepository, RoleRepository>();

            // config claim service
            //services.AddScoped<IClaimsService, ClaimsService>();
  
            // config mail service
            services.AddScoped<IMailService, MailService>();

            // config payos service
            services.AddScoped<IPayOSService, PayOSService>();

            // config payos service
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();

            // config notification service
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            // config part symptom service
            services.AddScoped<IPartSymptomRepository, PartSymptomRepository>();
            services.AddScoped<IPartSymptomService, PartSymptomService>();

            services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
            services.AddScoped<IChatMessageService, ChatMessageService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<IProductSymptomRepository, ProductSymptomRepository>();
            services.AddScoped<IProductSymptomService, ProductSymptomService>();

            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
