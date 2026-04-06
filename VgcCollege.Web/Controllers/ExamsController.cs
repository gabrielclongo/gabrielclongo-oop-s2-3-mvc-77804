using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Data;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔒 ADMIN / FACULTY
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Index()
        {
            var exams = await _context.Exams
                .Include(e => e.Course)
                .AsNoTracking()
                .ToListAsync();

            return View(exams);
        }

        // 👨‍🎓 STUDENT
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyExams()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var exams = await _context.Exams
                .Include(e => e.Course)
                .Where(e =>
                    e.ResultsReleased &&
                    _context.CourseEnrollments.Any(en =>
                        en.CourseId == e.CourseId &&
                        en.StudentProfile.IdentityUserId == userId))
                .AsNoTracking()
                .ToListAsync();

            return View("Index", exams);
        }

        // 🔓 TODOS LOGADOS
        [Authorize(Roles = "Admin,Faculty,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var exam = await _context.Exams
                .Include(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exam == null) return NotFound();

            return View(exam);
        }

        // 🔒 ADMIN / FACULTY
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            LoadCourses();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Create([Bind("Id,Title,CourseId,MaxScore,Date,ResultsReleased")] Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Exams.Add(exam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadCourses(exam);
            return View(exam);
        }

        // 🔒 ADMIN / FACULTY
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null) return NotFound();

            LoadCourses(exam);
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CourseId,MaxScore,Date,ResultsReleased")] Exam exam)
        {
            if (id != exam.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Exams.Update(exam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadCourses(exam);
            return View(exam);
        }

        // 🔒 ADMIN / FACULTY
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exam = await _context.Exams
                .Include(e => e.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exam == null) return NotFound();

            return View(exam);
        }

        // 🔒 SÓ ADMIN
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await _context.Exams.FindAsync(id);

            if (exam != null)
            {
                _context.Exams.Remove(exam);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadCourses(Exam? exam = null)
        {
            ViewData["CourseId"] = new SelectList(
                _context.Courses,
                "Id",
                "Name",
                exam?.CourseId
            );
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }
    }
}