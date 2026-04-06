using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ✅ LIST
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.IdentityUser)
                .AsNoTracking()
                .ToListAsync();

            return View(students);
        }

        // ✅ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .Include(s => s.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null) return NotFound();

            return View(student);
        }

        // ✅ GET CREATE
        public IActionResult Create()
        {
            LoadUsers();
            return View();
        }

        // ✅ POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentProfile student)
        {
            if (ModelState.IsValid)
            {
                // 🔒 evita duplicar profile para o mesmo user
                var exists = await _context.Students
                    .AnyAsync(s => s.IdentityUserId == student.IdentityUserId);

                if (exists)
                {
                    ModelState.AddModelError("", "This user already has a student profile.");
                    LoadUsers(student);
                    return View(student);
                }

                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadUsers(student);
            return View(student);
        }

        // ✅ GET EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            LoadUsers(student); // 🔥 ESSENCIAL
            return View(student);
        }

        // ✅ POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentProfile student)
        {
            if (id != student.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadUsers(student); // 🔥 ESSENCIAL
            return View(student);
        }

        // ✅ GET DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students
                .Include(s => s.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null) return NotFound();

            return View(student);
        }

        // ✅ POST DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔧 HELPER
        private void LoadUsers(StudentProfile? student = null)
        {
            ViewData["IdentityUserId"] = new SelectList(
                _context.Users,
                "Id",
                "Email",
                student?.IdentityUserId
            );
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}