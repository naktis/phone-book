using Api.RequestProcessors.DefaultSetters;
using Api.RequestProcessors.TokenExtractors;
using Business.Mappers;
using Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Api
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddTransient<IEntryParamsSetter, EntryParamsSetter>();
            services.AddTransient<IClaimExtractor, ClaimExtractor>();

            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<IEntryMapper, EntryMapper>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEntryService, EntryService>();

            return services;
        }
    }
}
