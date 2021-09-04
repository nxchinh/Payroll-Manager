using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payroll_Manager.Areas.Admin.Models.VM_DoanhNghiep;
using Payroll_Manager.Entity.ModelForAttendance;
using Payroll_Manager.Persistence;

namespace Payroll_Manager.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;    
        }
        private bool _disposed = false;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // GET: Attendances

        public async Task<IActionResult> Index()
        {
            return View(await _context.Attendance.OrderByDescending(x => x.Name).ToListAsync());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Index(List<Attendance> attendances, DateTime SearchString)
        {
            var filterresults = from x in _context.Attendance select x;
            if (DateTime.Compare(DateTime.MinValue, SearchString) == 0)
            {

                foreach (var i in attendances)
                {
                    var c = filterresults.FirstOrDefault(a => a.AttendanceID.Equals(i.AttendanceID));
                    if (c != null)
                    {
                     
                        c.Attendance_status = i.Attendance_status;
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
         
            if (SearchString != null)
            {
                filterresults = filterresults.Where(x => DateTime.Compare(x.Attendance_Date, SearchString)==0); 

                return View(filterresults.ToList());
            }
            return View(attendances);
            
        }
        // GET: Attendances/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance.SingleOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }
            else
            {
                AttendancesViewModel attendances = new AttendancesViewModel()
                {
                    AttendanceID = attendance.AttendanceID,
                    Attendance_Date = attendance.Attendance_Date,
                    Attendance_status = attendance.Attendance_status,
                    Name = attendance.Name
                };
                return View(attendances);
            }

        }

        // POST: Attendances/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AttendancesViewModel attendance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Attendance a = new Attendance()
                    {
                        Attendance_status = attendance.Attendance_status,
                        AttendanceID = attendance.AttendanceID,
                        Attendance_Date = attendance.Attendance_Date,
                        Name = attendance.Name
                    };
                    _context.Update(a);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.AttendanceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(attendance);
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .SingleOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .SingleOrDefaultAsync(m => m.AttendanceID == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendance.SingleOrDefaultAsync(m => m.AttendanceID == id);
            _context.Attendance.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendance.Any(e => e.AttendanceID == id);
        }
    }
}
