using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using ChatApplication.Server.Domain.Entities;
using ChatApplication.Server.Domain.DTOs.Common;
using ChatApplication.Server.MediatR;
using ChatApplication.Server.Persistence;
using ChatApplication.Server.Domain.Interfaces;
using ChatApplication.Server.Persistence.Repositories;
using ChatApplication.Server.Infrastructure.Services;

namespace ChatApplication.Server.Infrastructure
{
    public static class DependencyRegistration
    {
        public static void ConfigureAppServices(this IServiceCollection services, string assemblyName)
        {

            services.AddValidatorsFromAssemblies(new[] { typeof(ApplicationAssemblyReference).Assembly });
            services.AddMediatrToApp(assemblyName);
            services.RegisterServices(assemblyName);
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        private static void AddMediatrToApp(this IServiceCollection services, string assemblyName)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(ApplicationAssemblyReference).Assembly);

                // MediatR Pipeline Behaviors - outer most handler at the top, inner most handler at the bottom
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ErrorHandlerPipelineBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehavior<,>));

            });

            /*
             *  ErrorHandlerPipelineBehavior (global generic error handling)
             *    |
             *    |_ calls: LoggingPipelineBehavior (global generic logging)
             *          |
             *          |_ calls: FluentValidationPipelineBehavior (validation logic for Commands)
             *                |
             */
        }

        private static void RegisterServices(this IServiceCollection services, string assemblyName)
        {
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddScoped<IHttpUserContextService, HttpUserContextService>();
            services.AddScoped<IChatService, ChatService>();
        }
    }
}
