using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.EventDtos
{
    public class EventSearchDto
    {
        public string? City { get; set; }
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
        public string? SportType { get; set; }
    }
}
