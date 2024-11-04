using System;

namespace API.Installers;

public interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration Configuration);
}
