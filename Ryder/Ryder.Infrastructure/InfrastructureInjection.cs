using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ryder.Infrastructure.Common.Behaviours;
using Ryder.Infrastructure.Common.Extensions;
using Ryder.Infrastructure.Implementation;
using Ryder.Infrastructure.Interface;

namespace Ryder.Infrastructure
{
    public static class InfrastructureInjection
    {
        public static void InjectInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = new EmailSettings()
            {
                SmtpHost = configuration["EmailSettings:SmtpHost"],
                SmtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]),
                SmtpUsername = configuration["EmailSettings:SmtpUsername"],
                SmtpPassword = configuration["EmailSettings:SmtpPassword"],
                SenderName = configuration["EmailSettings:SenderName"],
                SenderEmail = configuration["EmailSettings:SenderEmail"]
            };
            services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ISmtpEmailService, SmtpEmailService>();
            services.AddSingleton(emailSettings);
            services.AddScoped<IPaystackService, PaystackService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}