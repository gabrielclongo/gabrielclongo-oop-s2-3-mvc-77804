using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;

namespace VgcCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty,Student")]
    public class GradebookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradebookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ VIEW GERAL (Admin / Faculty)
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Index()
        {
            var data = await _context.AssignmentResults
                .Include(a => a.StudentProfile)
                .Include(a => a.Assignment)
                    .ThenInclude(a => a.Course)
                .AsNoTracking()
                .ToListAsync();

            return View(data);
        }

        // ✅ VIEW DO PRÓPRIO ALUNO
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyGrades()
        {
            var userId = User?.Identity?.Name;

            var data = await _context.AssignmentResults
                .Include(a => a.StudentProfile)
                .Include(a => a.Assignment)
                    .ThenInclude(a => a.Course)
                .Where(a => a.StudentProfile.IdentityUserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return View("Index", data); // reutiliza mesma view
        }
    }
}