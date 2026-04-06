using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _context.AttendanceRecords
                .Include(a => a.CourseEnrollment)
                    .ThenInclude(e => e.StudentProfile)
                .ToListAsync();

            return View(data);
        }

        public IActionResult Create()
        {
            ViewData["CourseEnrollmentId"] = new SelectList(_context.CourseEnrollments, "Id", "Id");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttendanceRecord record)
        {
            _context.Add(record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}