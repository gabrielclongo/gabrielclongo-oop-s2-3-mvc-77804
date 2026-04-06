using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty,Student")]
    public class AssignmentResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 📊 GRADEBOOK (Assignments)
        public async Task<IActionResult> Index()
        {
            IQueryable<AssignmentResult> results = _context.AssignmentResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Assignment)
                .ThenInclude(a => a.Course);

            // 🔐 STUDENT → só vê os próprios
            if (User.IsInRole("Student"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.IdentityUserId == userId);

                if (student != null)
                {
                    results = results.Where(r => r.StudentProfileId == student.Id);
                }
            }

            return View(await results.ToListAsync());
        }

        // ➕ CREATE (Admin + Faculty)
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_context.Students, "Id", "Name");
            ViewBag.Assignments = new SelectList(_context.Assignments, "Id", "Title");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create(AssignmentResult result)
        {
            if (ModelState.IsValid)
            {
                _context.AssignmentResults.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Students = new SelectList(_context.Students, "Id", "Name", result.StudentProfileId);
            ViewBag.Assignments = new SelectList(_context.Assignments, "Id", "Title", result.AssignmentId);

            return View(result);
        }

        // ❌ DELETE
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.AssignmentResults
                .Include(r => r.StudentProfile)
                .Include(r => r.Assignment)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _context.AssignmentResults.FindAsync(id);

            if (result != null)
            {
                _context.AssignmentResults.Remove(result);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}