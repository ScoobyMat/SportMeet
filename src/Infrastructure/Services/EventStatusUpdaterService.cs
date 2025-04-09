using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class EventStatusUpdaterService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EventStatusUpdaterService> _logger;
    private readonly TimeSpan _delay = TimeSpan.FromMinutes(1);

    public EventStatusUpdaterService(IServiceScopeFactory scopeFactory, ILogger<EventStatusUpdaterService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("EventStatusUpdaterService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var eventRepository = scope.ServiceProvider.GetRequiredService<IEventRepository>();
                    var events = await eventRepository.GetAllAsync();

                    var now = DateTime.Now;
                    var today = DateOnly.FromDateTime(now);
                    var currentTime = now.TimeOfDay;

                    foreach (var ev in events)
                    {
                        if ((ev.Date < today || (ev.Date == today && ev.Time < currentTime))
                            && ev.Status != EventStatus.Completed
                            && ev.Status != EventStatus.Cancelled)
                        {
                            ev.Status = EventStatus.Completed;
                            await eventRepository.UpdateAsync(ev);
                            _logger.LogInformation($"Updated event {ev.Id} status to Completed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event status.");
            }

            await Task.Delay(_delay, stoppingToken);
        }

        _logger.LogInformation("EventStatusUpdaterService stopped.");
    }
}
