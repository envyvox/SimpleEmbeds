using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SE.Services.Client;

namespace SE.Services.Extensions
{
    public static class DiscordExtensions
    {
        public static IApplicationBuilder StartDiscord(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var service = serviceScope.ServiceProvider.GetService<IDiscordClientService>();
            service.Start().Wait();

            return app;
        }
    }
}
