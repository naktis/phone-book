using Business.Mappers;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IUserService, AccountService>();
            services.AddTransient<IAccountMapper, AccountMapper>();
            return services;
        }
    }
}
