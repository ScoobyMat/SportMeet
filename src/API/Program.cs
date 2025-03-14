using API.Installers;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
IConfiguration configuration = builder.Configuration;
builder.Services.InstallServicesInAssembly(configuration);

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("/hubs/presence").RequireAuthorization();
app.MapHub<EventChatHub>("/hubs/eventChatHub").RequireAuthorization();
app.MapHub<PrivateChatHub>("/hubs/privateChatHub").RequireAuthorization();

app.Run();

public partial class Program { }
