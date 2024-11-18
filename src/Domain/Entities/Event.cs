using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Event : BaseDomainEntity
    {
        public required string EventName { get; set; }
        public required string Location { get; set; }
        public required DateTime Date { get; set; }

        /*public string Address { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }*/

        public int CreatedByUserId { get; set; }
        public AppUser CreatedByUser { get; set; } = null!;

        public Group Group { get; set; } = null!;
    }
}
