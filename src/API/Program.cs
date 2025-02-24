using API.Installers;
using API.SignalR;
using Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = builder.Configuration;
builder.Services.InstallServicesInAssembly(configuration);

builder.Services.Configure<CloudinarySettings>
    (builder.Configuration.GetSection("CloudinarySettings"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<PresenceTracker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.MapHub<PresenceHub>("hubs/presence");
//app.MapHub<ChatHub>("/chatHub");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
