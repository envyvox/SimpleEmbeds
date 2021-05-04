using System.Reflection;
using Autofac;
using Discord.Commands;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SE.Framework.Autofac;
using SE.Services.Services.DiscordClientService;
using SE.Services.Services.DiscordClientService.Impl;
using SE.Services.Services.DiscordClientService.Options;

namespace SE
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DiscordClientOptions>(x => _configuration.GetSection("Discord").Bind(x));
            services.AddSingleton<CommandService>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.StartDiscord();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var assembly = typeof(IDiscordClientService).Assembly;
            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsDefined(typeof(InjectableServiceAttribute), false) &&
                            x.GetCustomAttribute<InjectableServiceAttribute>().IsSingletone)
                .As(x => x.GetInterfaces()[0])
                .SingleInstance();

            builder.RegisterAssemblyTypes(assembly)
                .Where(x => x.IsDefined(typeof(InjectableServiceAttribute), false) &&
                            !x.GetCustomAttribute<InjectableServiceAttribute>().IsSingletone)
                .As(x => x.GetInterfaces()[0])
                .InstancePerLifetimeScope();
        }
    }
}
