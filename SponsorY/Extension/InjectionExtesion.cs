using SponsorY.DataAccess.Survices.Contract;
using SponsorY.DataAccess.Survices;

namespace SponsorY.Extension
{
    public static class InjectionExtesion
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IServiceYoutub, ServiceYoutube>();
            services.AddScoped<IServiceUser, ServiceUser>();
            services.AddScoped<IServiceSponsorship, ServiceSponsorship>();
            services.AddScoped<IServiceCategory, ServiceCategory>();
            services.AddScoped<IServiceTransaction, ServiceTransaction>();
            services.AddScoped<IServiceMenu, ServiceMenu>();
            services.AddScoped<IServiceAdmin, ServiceAdmin>();
            services.AddScoped<IServiceSettings, ServiceSettings>();

            return services;
        }
    }
}
