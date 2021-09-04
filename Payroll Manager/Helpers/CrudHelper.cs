using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payroll_Manager.Areas.Admin.Models.VM_Event;
using Payroll_Manager.Entity.ModelForUser;

namespace Payroll_Manager.Helpers
{
    public static class CrudHelper
    { 
        public static Event ToEntity(this EventViewModel model)
        {
            return new Event { Id = model.Id, TenSuKien = model.TenSuKien, ThoiGianBatDau = model.ThoiGianBatDau, Active = model.Active };
        }
        public static EventViewModel ToModel(this Event model)
        {
            return new EventViewModel { Id = model.Id, TenSuKien = model.TenSuKien, ThoiGianBatDau = model.ThoiGianBatDau, Active = model.Active };
        }
        public static List<EventViewModel> ToModelList(this List<Event> entitiesList)
        {
            List<EventViewModel> modelsList = new List<EventViewModel>();
            foreach (var entity in entitiesList)
            {
                modelsList.Add(entity.ToModel());
            }
            modelsList.TrimExcess();
            return modelsList;
        }
    }
}
