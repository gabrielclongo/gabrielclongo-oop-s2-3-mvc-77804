using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var exams = _context.Exams.Include(e => e.Course);
            return View(await exams.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CourseId = new SelectList(_context.Courses, "Id", "Name", exam.CourseId);
            return View(exam);
        }
    }
}