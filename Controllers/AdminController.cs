using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;
using DentalClinic.Models.DTO;

namespace DentalClinic.Controllers
{
    [Route("admin")]
    public class AdminController : BaseController
    {
        private readonly UserManager<Profile> _userManager;

        public AdminController(DatabaseContext context, UserManager<Profile> userManager) : base(context)
        {
            _userManager = userManager;
        }

        private bool IsAdmin() => User.Identity!.IsAuthenticated && GetProfile().IsAdmin;

        [HttpGet]
        [Route("appointments")]
        public IActionResult Index()
        {
            if (!IsAdmin()) return Forbid();

            var appointments = _context.Appointments
                .OrderByDescending(a => a.StartAt)
                .ToList();

            return View("Appointments/Index", appointments);
        }

        [HttpGet]
        [Route("appointments/{appointmentId}")]
        public IActionResult Appointment(long appointmentId)
        {
            if (!IsAdmin()) return Forbid();

            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment == null) return NotFound();

            return View("Appointments/Edit", appointment);
        }

        [HttpPost]
        [Route("appointments/{appointmentId}")]
        public IActionResult AppointmentUpdate(long appointmentId, Appointment appointment)
        {
            if (!IsAdmin()) return Forbid();

            var existing = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (existing == null) return NotFound();

            existing.StaffId = appointment.StaffId;
            existing.ClientId = appointment.ClientId;
            existing.StartAt = appointment.StartAt;
            existing.Duration = appointment.Duration;
            existing.Recommendation = appointment.Recommendation;
            existing.DurationChangeReason = appointment.DurationChangeReason;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("appointments/{appointmentId}/delete")]
        public IActionResult AppointmentDelete(long appointmentId)
        {
            if (!IsAdmin()) return Forbid();

            var appointment = _context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("profiles")]
        public IActionResult Profiles()
        {
            if (!IsAdmin()) return Forbid();

            var profiles = _context.Profiles.ToList();
            return View("Profiles/Index", profiles);
        }

        [HttpGet]
        [Route("profiles/{profileId}")]
        public IActionResult Profile(long profileId)
        {
            if (!IsAdmin()) return Forbid();

            var profile = _context.Profiles.FirstOrDefault(p => p.Id == profileId);
            if (profile == null) return NotFound();

            return View("Profiles/Edit", profile);
        }

        [HttpGet]
        [Route("profiles/create")]
        public IActionResult ProfileCreate()
        {
            if (!IsAdmin()) return Forbid();
            return View("Profiles/Create");
        }

        [HttpPost]
        [Route("profiles/create")]
        public async Task<IActionResult> ProfileStore(NewProfile model)
        {
            if (!IsAdmin()) return Forbid();

            if (!ModelState.IsValid)
                return View("Profiles/Create", model);

            long roleId = model.RoleTitle switch
            {
                RoleTitle.Администратор => 2,
                RoleTitle.Менеджер     => 3,
                RoleTitle.Доктор       => 4,
                _                      => 1
            };

            var profile = new Profile
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                EmailConfirmed = true,
                RoleId = roleId
            };

            var result = await _userManager.CreateAsync(profile, model.Password);
            if (result.Succeeded)
            {
                _context.SaveChanges();
                return RedirectToAction("Profiles");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View("Profiles/Create", model);
        }

        [HttpPost]
        [Route("profiles/{profileId}")]
        public IActionResult ProfileUpdate(long profileId, Profile updated)
        {
            if (!IsAdmin()) return Forbid();

            var profile = _context.Profiles.FirstOrDefault(p => p.Id == profileId);
            if (profile == null) return NotFound();

            profile.UserName = updated.UserName;
            profile.Email = updated.Email;
            profile.PhoneNumber = updated.PhoneNumber;
            _context.SaveChanges();

            return RedirectToAction("Profiles");
        }

        [HttpPost]
        [Route("profiles/{profileId}/delete")]
        public IActionResult ProfileDelete(long profileId)
        {
            if (!IsAdmin()) return Forbid();

            var profile = _context.Profiles.FirstOrDefault(p => p.Id == profileId);
            if (profile == null) return NotFound();

            _context.Profiles.Remove(profile);
            _context.SaveChanges();

            return RedirectToAction("Profiles");
        }

        [HttpPost]
        [Route("profiles/{profileId}/ban")]
        public IActionResult Ban(long profileId)
        {
            if (!IsAdmin()) return Forbid();

            var user = _context.Profiles.FirstOrDefault(p => p.Id == profileId);
            if (user == null) return NotFound();

            user.EmailConfirmed = false;
            _context.SaveChanges();

            return RedirectToAction("Profiles");
        }

        [HttpPost]
        [Route("profiles/{profileId}/unban")]
        public IActionResult Unban(long profileId)
        {
            if (!IsAdmin()) return Forbid();

            var user = _context.Profiles.FirstOrDefault(p => p.Id == profileId);
            if (user == null) return NotFound();

            user.EmailConfirmed = true;
            _context.SaveChanges();

            return RedirectToAction("Profiles");
        }

        [HttpGet]
        [Route("reviews")]
        public IActionResult Reviews()
        {
            if (!IsAdmin()) return Forbid();

            var reviews = _context.Reviews.ToList();
            return View("Reviews/Index", reviews);
        }

        [HttpGet]
        [Route("reviews/{reviewId}")]
        public IActionResult Review(long reviewId)
        {
            if (!IsAdmin()) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null) return NotFound();

            return View("Reviews/Edit", review);
        }

        [HttpPost]
        [Route("reviews/{reviewId}")]
        public IActionResult ReviewUpdate(long reviewId, Review updated)
        {
            if (!IsAdmin()) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null) return NotFound();

            review.Text = updated.Text;
            _context.SaveChanges();

            return RedirectToAction("Reviews");
        }

        [HttpPost]
        [Route("reviews/{reviewId}/delete")]
        public IActionResult ReviewDelete(long reviewId)
        {
            if (!IsAdmin()) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }

            return RedirectToAction("Reviews");
        }

        [HttpPost]
        [Route("reviews/{reviewId}/show")]
        public IActionResult ShowReview(long reviewId)
        {
            if (!IsAdmin()) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null) return NotFound();

            review.IsVisible = true;
            _context.SaveChanges();

            return RedirectToAction("Reviews");
        }

        [HttpPost]
        [Route("reviews/{reviewId}/hide")]
        public IActionResult HideReview(long reviewId)
        {
            if (!IsAdmin()) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null) return NotFound();

            review.IsVisible = false;
            _context.SaveChanges();

            return RedirectToAction("Reviews");
        }
    }
}
