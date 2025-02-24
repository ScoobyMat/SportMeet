using Application.Dtos.EventAttendeeDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEventAttendeeService
    {
        Task<EventAttendeeDto?> GetByIdAsync(int id);
        Task<IEnumerable<EventAttendeeDto>> GetByEventIdAsync(int eventId);
        Task<EventAttendeeDto> AddAttendeeAsync(EventAttendeeCreateDto dto);
        Task UpdateAttendeeAsync(EventAttendeeUpdateDto dto);
        Task DeleteAttendeeAsync(int id);
    }
}
