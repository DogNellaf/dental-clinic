using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;

namespace DentalClinic.Controllers
{
    [Route("client")]
    public class ClientController : BaseController
    {
        public ClientController(DatabaseContext context) : base(context) { }

        [HttpGet]
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsClient) return Forbid();

            var appointments = _context.Appointments
                .Where(a => a.ClientId == profile.Id)
                .OrderByDescending(a => a.StartAt)
                .ToList();

            return View(appointments);
        }

        [HttpGet]
        [Route("review")]
        public IActionResult Review()
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsClient) return Forbid();

            var hasPastAppointment = _context.Appointments
                .Any(a => a.ClientId == profile.Id && a.StartAt < DateTime.Now);

            if (!hasPastAppointment)
                return View("Review/DeniedMessage");

            var review = _context.Reviews.FirstOrDefault(r => r.ProfileId == profile.Id);
            return View("Review/Edit", review);
        }

        [HttpGet]
        [Route("appointments/{appointmentId}")]
        public IActionResult Appointment(long appointmentId)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsClient) return Forbid();

            var appointment = _context.Appointments
                .FirstOrDefault(a => a.Id == appointmentId && a.ClientId == profile.Id);

            if (appointment == null)
                return NotFound();

            return View("Appointment", appointment);
        }

        [HttpPost]
        [Route("review/create")]
        public IActionResult ReviewCreate(Review review)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsClient) return Forbid();

            if (!ModelState.IsValid)
                return View("Review/Edit", review);

            var existingReview = _context.Reviews.FirstOrDefault(r => r.ProfileId == profile.Id);
            if (existingReview != null)
            {
                existingReview.Text = review.Text;
                existingReview.IsVisible = false;
            }
            else
            {
                review.IsVisible = false;
                review.ProfileId = profile.Id;
                _context.Reviews.Add(review);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("review/hide")]
        public IActionResult Hide()
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsClient) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.ProfileId == profile.Id);
            if (review == null)
                return NotFound();

            review.IsVisible = false;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
