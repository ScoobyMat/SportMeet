using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.EventAttendeeDtos
{
    public class EventAttendeeCreateDto
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; } = "Participant";
    }
}
