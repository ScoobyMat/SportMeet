using System.Reflection;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IEventAttendeeService, EventAttendeeService>();
        services.AddScoped<IFriendshipService, FriendshipService>();
        services.AddScoped<IEventMessageService, EventMessageService>();
        services.AddScoped<IPrivateMessageService, PrivateMessageService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
