using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend_reactNative_Shoppee_Services.Interfaces;
using Backend_reactNative_Shoppee_Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Backend_reactNative_Shoppee_Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddScoped<IUserServices, UserServices>();
            return service;
        }
    }
}
