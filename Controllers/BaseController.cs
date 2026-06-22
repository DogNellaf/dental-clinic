using Microsoft.AspNetCore.Mvc;
using DentalClinic.Models;

namespace DentalClinic.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly DatabaseContext _context;

        protected BaseController(DatabaseContext context)
        {
            _context = context;
        }

        protected Profile GetProfile()
        {
            return _context.Profiles.First(p => p.UserName == User.Identity!.Name);
        }
    }
}
