using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ LIST
        public async Task<IActionResult> Index()
        {
            var courses = _context.Courses
                .Include(c => c.Branch);

            return View(await courses.ToListAsync());
        }

        // ✅ GET CREATE
        public IActionResult Create()
        {
            LoadBranches();
            return View();
        }

        // ✅ POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadBranches(course);
            return View(course);
        }



        // 🔥 MÉTODO AUXILIAR (IMPORTANTE)
        private void LoadBranches(Course? course = null)
        {
            ViewData["BranchId"] = new SelectList(
                _context.Branches,
                "Id",
                "Name",
                course?.BranchId
            );
        }
    }
}