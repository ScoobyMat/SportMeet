using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Group : BaseDomainEntity
    {
        public int ManagedByUserId { get; set; }
        public User ManagedByUser { get; set; } = null!;

        public Event Event { get; set; } = null!;
        public int EventId { get; set; }
        
        public List<GroupMember> Members { get; set; } = [];
    }
}
