using Application.Interfaces;
using Infrastructure.Mail;
using Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<ISendConfirmationEmail, SendMail>();
            return services;
        }
    }
}