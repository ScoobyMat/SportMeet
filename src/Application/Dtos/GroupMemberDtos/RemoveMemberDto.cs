using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.GroupMemberDtos
{
    public class RemoveMemberDto
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
    }
}
