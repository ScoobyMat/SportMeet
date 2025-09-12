using Domain.Entities;

namespace Application.Interfaces
{
    public interface IGeoCodingService
    {
        Task<Event?> GetCoordinatesAsync(Event newEvent);
    }
}
