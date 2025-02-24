using Application.Dtos.PrivateMessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPrivateMessageService
    {
        Task<IEnumerable<PrivateMessageDto>> GetConversationAsync(int userAId, int userBId);
        Task<PrivateMessageDto> SendMessageAsync(PrivateMessageCreateDto dto);
        Task DeleteMessageAsync(int messageId);
    }
}
