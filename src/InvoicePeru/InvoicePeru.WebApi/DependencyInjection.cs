using InvoicePeru.WebApi.Common;
using InvoicePeru.WebApi.Service;

namespace InvoicePeru.WebApi;
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMappings();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<JwtService>();
        return services;
    }
}