using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Payroll_Manager.Controllers
{
    public class TestFrontEndController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}