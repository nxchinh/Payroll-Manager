using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Entity;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Services.Implementation
{
    public class AddressService: IAddressService
    {
        private readonly ApplicationDbContext _context;

        public AddressService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }
        public IEnumerable<Address> GetAll() => _context.Addresses.AsNoTracking().OrderBy(add => add.City);
        public Address GetById(int addressId) => _context.Addresses.FirstOrDefault(add => add.Id == addressId);

        public async Task UpdateAsync(Address address)
        {
            _context.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int addressId)
        {
            var address = GetById(addressId);
            _context.Remove(address);
            await _context.SaveChangesAsync();
        }


        public IEnumerable<SelectListItem> GetAllAddress()
        {
            return GetAll().Select(add => new SelectListItem()
            {
                Text = add.FullAddress,
                Value = add.Id.ToString()
            });
        }
    }
}
