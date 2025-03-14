using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IEventAttendeeRepository, EventAttendeeRepository>();
        services.AddScoped<IEventMessageRepository, EventMessageRepository>();
        services.AddScoped<IFriendshipRepository, FriendshipRepository>();
        services.AddScoped<IPrivateMessageRepository, PrivateMessageRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.Configure<CloudinarySettings>(options =>
            configuration.GetSection("CloudinarySettings").Bind(options));

        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IGeoCodingService, GeoCodingService>();


        return services;
    }
}
