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

        public async Task<IActionResult> Index()
        {
            var data = await _context.AssignmentResults
                .Include(a => a.StudentProfile)
                .Include(a => a.Assignment)
                    .ThenInclude(a => a.Course)
                .ToListAsync();

            return View(data);
        }
    }
}