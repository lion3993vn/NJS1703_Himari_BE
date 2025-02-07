using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWD392_Himari.Repository.Interfaces;
using SWD392_Himari.Repository.Repository;

namespace SWD392_Himari.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection service)
        {
            service.AddScoped<IUserRepository, UserRepository>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            return service;
        }
    }
}
