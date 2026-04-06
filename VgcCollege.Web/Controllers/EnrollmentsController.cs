using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = _context.CourseEnrollments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course);

            return View(await data.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_context.Students, "Id", "Name");
            ViewBag.Courses = new SelectList(_context.Courses, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseEnrollment enrolment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrolment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enrolment);
        }
    }
}