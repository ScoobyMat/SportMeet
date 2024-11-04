using System;
using Application;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;

namespace API.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration Configuration)
    {
        services.AddApplication();
        services.AddInfrastructure();
        services.AddControllers();
    }
}
