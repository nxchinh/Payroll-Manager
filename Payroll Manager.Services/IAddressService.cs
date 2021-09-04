using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Payroll_Manager.Entity;

namespace Payroll_Manager.Services
{
    public interface IAddressService
    {
        Task CreateAsync(Address address);

        Address GetById(int addressId);
        Task UpdateAsync(Address address);
        Task Delete(int addressId);
        IEnumerable<Address> GetAll();
        IEnumerable<SelectListItem> GetAllAddress();
    }
}
