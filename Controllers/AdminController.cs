using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    [Route("admin_102809")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new AdminViewModel
            {
                Projects = await _context.Projects.OrderByDescending(p => p.CreatedAt).ToListAsync(),
                Skills = await _context.Skills.ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Project project)
        {
            if (ModelState.IsValid)
            {
                project.CreatedAt = DateTime.UtcNow;
                _context.Add(project);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Proje başarıyla eklendi!";
                return RedirectToAction(nameof(Index));
            }
            var viewModel = new AdminViewModel
            {
                Projects = await _context.Projects.OrderByDescending(p => p.CreatedAt).ToListAsync(),
                Skills = await _context.Skills.ToListAsync()
            };
            return View(viewModel);
        }

        [HttpPost("AddSkill")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSkill(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag))
            {
                var skill = new Skill { Tag = tag };
                _context.Skills.Add(skill);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Yetenek başarıyla eklendi!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("DeleteSkill")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill != null)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Yetenek başarıyla silindi!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Proje başarıyla güncellendi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Proje başarıyla silindi!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
