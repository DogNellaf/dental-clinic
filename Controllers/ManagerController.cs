using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;

namespace DentalClinic.Controllers
{
    [Route("manager")]
    public class ManagerController : BaseController
    {
        public ManagerController(DatabaseContext context) : base(context) { }

        [HttpGet]
        [Route("reviews/all")]
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsManager) return Forbid();

            var reviews = _context.Reviews.Where(r => r.IsVisible).ToList();
            return View(reviews);
        }

        [HttpGet]
        [Route("reviews/hidden")]
        public IActionResult HiddenReviews()
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsManager) return Forbid();

            var reviews = _context.Reviews.Where(r => !r.IsVisible).ToList();
            return View(reviews);
        }

        [HttpPost]
        [Route("reviews/{reviewId}/show")]
        public IActionResult Show(long reviewId)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsManager) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null) return NotFound();

            review.IsVisible = true;
            _context.SaveChanges();

            return RedirectToAction("HiddenReviews");
        }

        [HttpPost]
        [Route("reviews/{reviewId}/hide")]
        public IActionResult Hide(long reviewId)
        {
            if (!User.Identity!.IsAuthenticated) return Forbid();

            var profile = GetProfile();
            if (!profile.IsManager) return Forbid();

            var review = _context.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null) return NotFound();

            review.IsVisible = false;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
