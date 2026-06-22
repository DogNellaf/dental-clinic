using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using DentalClinic.Models;

namespace DentalClinic.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context) : base(context)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            var reviews = _context.Reviews
                .Where(r => r.IsVisible)
                .OrderByDescending(r => r.Id)
                .Take(5)
                .ToList();

            return View(reviews);
        }

        [HttpGet]
        [Route("faq")]
        public IActionResult FAQ()
        {
            return View();
        }

        [HttpGet]
        [Route("schedule")]
        public IActionResult Schedule()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        [HttpGet]
        [Route("schedule/services")]
        public IActionResult ScheduleWithService([FromQuery(Name = "Service")] string? serviceTitle)
        {
            if (string.IsNullOrEmpty(serviceTitle))
                return RedirectToAction("Schedule");

            var service = _context.Services
                .Include(s => s.Staff)
                .FirstOrDefault(s => s.Title == serviceTitle);

            if (service == null)
                return RedirectToAction("Schedule");

            return View(service.Staff);
        }

        [HttpGet]
        [Route("schedule/staff")]
        public IActionResult ScheduleWithStaff([FromQuery(Name = "Staff")] string? staffLogin)
        {
            if (string.IsNullOrEmpty(staffLogin))
                return RedirectToAction("Schedule");

            var staff = _context.Staffs.FirstOrDefault(s => s.ExternalLogin == staffLogin);
            if (staff == null)
                return RedirectToAction("Schedule");

            var appointments = _context.Appointments
                .Where(a => a.StaffId == staff.Id && a.ClientId == 0 && a.StartAt > DateTime.Now)
                .ToList();

            return View(appointments);
        }

        [HttpGet]
        [Route("appointments/add")]
        public IActionResult AddAppointment([FromQuery(Name = "Appointment")] long? appointmentId)
        {
            if (appointmentId == null)
                return RedirectToAction("Schedule");

            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("LoginPage", "Auth");

            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment == null)
                return NotFound();

            if (appointment.IsBooked)
            {
                TempData["Error"] = "Этот приём уже занят. Пожалуйста, выберите другое время.";
                return RedirectToAction("Schedule");
            }

            var profile = GetProfile();
            appointment.ClientId = profile.Id;
            _context.SaveChanges();

            return RedirectToAction("Index", "Client");
        }

        [HttpGet]
        [Route("services")]
        public IActionResult Services()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        [HttpGet]
        [Route("services/{serviceId}")]
        public IActionResult ServiceDetail(long serviceId)
        {
            var service = _context.Services.FirstOrDefault(s => s.Id == serviceId);
            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpGet]
        [Route("contacts")]
        public IActionResult Contacts()
        {
            return View();
        }

        [HttpGet]
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
