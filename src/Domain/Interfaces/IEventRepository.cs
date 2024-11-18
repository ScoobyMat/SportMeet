using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetEvents();
        Task<Event?> GetEvent(int id);
        Task Add(Event AddEvent);
        void UpdateEvent(Event UpdateEvent);
        void DeleteEvent(Event DeleteEvent);
    }
}
