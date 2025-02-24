using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.NotificationDtos
{
    public class NotificationCreateDto
    {
        public int UserId { get; set; }
        public string Type { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}
