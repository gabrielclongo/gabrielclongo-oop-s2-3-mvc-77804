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
    [Authorize(Roles = "Admin,Faculty")] // 🔒 PROTEÇÃO
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ LIST
        public async Task<IActionResult> Index()
        {
            var enrollments = _context.CourseEnrollments
                .Include(e => e.Course)
                .Include(e => e.StudentProfile);

            return View(await enrollments.ToListAsync());
        }

        // ✅ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.CourseEnrollments
                .Include(e => e.Course)
                .Include(e => e.StudentProfile)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // ✅ GET CREATE
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        // ✅ POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentProfileId,CourseId")] CourseEnrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(enrollment);
            return View(enrollment);
        }

        // ✅ GET EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.CourseEnrollments.FindAsync(id);
            if (enrollment == null) return NotFound();

            LoadDropdowns(enrollment);
            return View(enrollment);
        }

        // ✅ POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentProfileId,CourseId")] CourseEnrollment enrollment)
        {
            if (id != enrollment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseEnrollmentExists(enrollment.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns(enrollment);
            return View(enrollment);
        }

        // ✅ GET DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.CourseEnrollments
                .Include(e => e.Course)
                .Include(e => e.StudentProfile)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // ✅ POST DELETE (🔒 só Admin)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.CourseEnrollments.FindAsync(id);

            if (enrollment != null)
            {
                _context.CourseEnrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ MÉTODO AUXILIAR (clean code)
        private void LoadDropdowns(CourseEnrollment? enrollment = null)
        {
            ViewData["CourseId"] = new SelectList(
                _context.Courses,
                "Id",
                "Name",
                enrollment?.CourseId
            );

            ViewData["StudentProfileId"] = new SelectList(
                _context.Students,
                "Id",
                "Name",
                enrollment?.StudentProfileId
            );
        }

        private bool CourseEnrollmentExists(int id)
        {
            return _context.CourseEnrollments.Any(e => e.Id == id);
        }
    }
}