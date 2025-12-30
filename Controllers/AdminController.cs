using Microsoft.AspNetCore.Mvc;
using Portfolio.Services;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    [Route("admin_102809")]
    public class AdminController : Controller
    {
        private readonly IProjectService _projectService;

        public AdminController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var viewModel = new AdminViewModel
            {
                Projects = projects.OrderByDescending(p => p.CreatedAt).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Project project)
        {
            if (ModelState.IsValid)
            {
                await _projectService.SaveProjectAsync(project);
                TempData["Success"] = "Project added successfully!";
                return RedirectToAction(nameof(Index));
            }
            
            var projects = await _projectService.GetAllProjectsAsync();
            var viewModel = new AdminViewModel
            {
                Projects = projects.OrderByDescending(p => p.CreatedAt).ToList()
            };
            return View(viewModel);
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var project = await _projectService.GetProjectByIdAsync(id.Value);
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
                await _projectService.UpdateProjectAsync(project);
                TempData["Success"] = "Project updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            TempData["Success"] = "Project deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
