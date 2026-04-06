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
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ LIST
        public async Task<IActionResult> Index()
        {
            var assignments = _context.Assignments
                .Include(a => a.Course);

            return View(await assignments.ToListAsync());
        }

        // ✅ DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var assignment = await _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (assignment == null) return NotFound();

            return View(assignment);
        }

        // ✅ GET CREATE
        public IActionResult Create()
        {
            LoadCourses();
            return View();
        }

        // ✅ POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CourseId,MaxScore")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            LoadCourses(assignment);
            return View(assignment);
        }

        // ✅ GET EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null) return NotFound();

            LoadCourses(assignment);
            return View(assignment);
        }

        // ✅ POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CourseId,MaxScore")] Assignment assignment)
        {
            if (id != assignment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadCourses(assignment);
            return View(assignment);
        }

        // ✅ GET DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var assignment = await _context.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (assignment == null) return NotFound();

            return View(assignment);
        }

        // ✅ POST DELETE (🔒 só Admin pode apagar)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);

            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ✅ MÉTODO AUXILIAR (clean code)
        private void LoadCourses(Assignment? assignment = null)
        {
            ViewData["CourseId"] = new SelectList(
                _context.Courses,
                "Id",
                "Name",
                assignment?.CourseId
            );
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.Id == id);
        }
    }
}