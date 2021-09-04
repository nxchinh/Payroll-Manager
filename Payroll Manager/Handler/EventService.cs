using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Payroll_Manager.Entity.ModelForUser;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Handler
{
    public class EventService
    {
        private readonly ApplicationDbContext context;

        public EventService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public Event Add(Event entity)
        {
            entity.Active = true;
            context.Add(entity);
            context.SaveChanges();
            return entity;
        }
        public Event Restore(int idToSearch)
        {
            Event found = null;
            found = context.Find<Event>(idToSearch);
            found.Active = true;
            context.Update(found);
            context.SaveChanges();
            return found;
        }
        public Event Delete(int idToSearch)
        {
            Event found = null;
            found = context.Find<Event>(idToSearch);
            found.Active = false;
            context.Update(found);
            context.SaveChanges();
            return found;
        }
        public Event Update(int idToSearch, Event entity)
        {
            Event found = null;
            found = context.Find<Event>(idToSearch);
            found.Id = entity.Id;
            if (!string.IsNullOrEmpty(entity.TenSuKien))
            {
                found.TenSuKien = entity.TenSuKien;
            }
            if (!string.IsNullOrEmpty(entity.ThoiGianBatDau.ToString(CultureInfo.InvariantCulture)))
            {
                found.ThoiGianBatDau = entity.ThoiGianBatDau;
            }
            context.SaveChanges();
            return entity;
        }
        public List<Event> Get() => (from c in context.Events select c).ToList();

        public Event Getone(int id) =>
            (from c in context.Events
                where c.Id == id
                select c).FirstOrDefault();
    }
}
