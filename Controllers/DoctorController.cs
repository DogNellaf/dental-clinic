using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;

namespace DentalClinic.Controllers
{
    [Route("doctor")]
    public class DoctorController : BaseController
    {
        public DoctorController(DatabaseContext context) : base(context) { }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsDoctor) return Forbid();

            var now = DateTime.Now;
            var staff = _context.Staffs.FirstOrDefault(s => s.Profile.Id == profile.Id);
            if (staff == null) return NotFound();

            var current = _context.Appointments
                .FirstOrDefault(a => a.StaffId == staff.Id && now >= a.StartAt && now <= a.StartAt.AddMinutes(a.Duration));

            return View(current);
        }

        [HttpGet]
        [Route("appointments")]
        public IActionResult Appointments()
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsDoctor) return Forbid();

            var staff = _context.Staffs.FirstOrDefault(s => s.Profile.Id == profile.Id);
            if (staff == null) return NotFound();

            var appointments = _context.Appointments
                .Where(a => a.StaffId == staff.Id)
                .OrderBy(a => a.StartAt)
                .ToList();

            return View(appointments);
        }

        [HttpPost]
        [Route("appointments/{appointmentId}/recommend")]
        public IActionResult AddRecommendation(long appointmentId, string recommendation)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsDoctor) return Forbid();

            var staff = _context.Staffs.FirstOrDefault(s => s.Profile.Id == profile.Id);
            if (staff == null) return NotFound();

            var appointment = _context.Appointments
                .FirstOrDefault(a => a.Id == appointmentId && a.StaffId == staff.Id);
            if (appointment == null) return NotFound();

            appointment.Recommendation = recommendation ?? string.Empty;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("appointments/{appointmentId}/extend")]
        public IActionResult ExtendAppointment(long appointmentId, short additionalMinutes, string reason)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsDoctor) return Forbid();

            var staff = _context.Staffs.FirstOrDefault(s => s.Profile.Id == profile.Id);
            if (staff == null) return NotFound();

            var appointment = _context.Appointments
                .FirstOrDefault(a => a.Id == appointmentId && a.StaffId == staff.Id);
            if (appointment == null) return NotFound();

            if (additionalMinutes <= 0)
            {
                TempData["Error"] = "Укажите корректное количество минут для продления.";
                return RedirectToAction("Index");
            }

            appointment.Duration += additionalMinutes;
            appointment.DurationChangeReason = reason ?? string.Empty;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("appointments/{appointmentId}/end")]
        public IActionResult EndAppointmentEarly(long appointmentId, string reason)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsDoctor) return Forbid();

            var staff = _context.Staffs.FirstOrDefault(s => s.Profile.Id == profile.Id);
            if (staff == null) return NotFound();

            var appointment = _context.Appointments
                .FirstOrDefault(a => a.Id == appointmentId && a.StaffId == staff.Id);
            if (appointment == null) return NotFound();

            var now = DateTime.Now;
            if (now > appointment.StartAt)
            {
                appointment.Duration = (short)(now - appointment.StartAt).TotalMinutes;
                appointment.DurationChangeReason = reason ?? string.Empty;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("clients/{clientId}/record")]
        public IActionResult ClientRecord(long clientId)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsDoctor) return Forbid();

            var appointments = _context.Appointments
                .Where(a => a.ClientId == clientId)
                .OrderByDescending(a => a.StartAt)
                .ToList();

            ViewBag.ClientId = clientId;
            return View("ClientRecord", appointments);
        }
    }
}
