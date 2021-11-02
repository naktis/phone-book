using Api.RequestProcessors.DefaultSetters;
using Api.RequestProcessors.TokenExtractors;
using Api.RequestProcessors.Validators;
using Api.RequestProcessors.Validators.Interfaces;
using Business.Mappers;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<ISharedValidator, SharedValidator>();
            services.AddTransient<IKeyValidator, KeyValidator>();
            services.AddTransient<IEntryParamsValidator, EntryParamsValidator>();
            services.AddTransient<IEntryValidator, EntryValidator>();
            services.AddTransient<IUserValidator, UserValidator>();

            services.AddTransient<IEntryParamsSetter, EntryParamsSetter>();
            services.AddTransient<IClaimExtractor, ClaimExtractor>();

            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<IEntryMapper, EntryMapper>();

            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEntryService, EntryService>();

            return services;
        }
    }
}
