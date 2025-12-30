using Microsoft.AspNetCore.Mvc;
using Portfolio.Services;
using Portfolio.Models;

namespace Portfolio.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return View(projects.OrderByDescending(p => p.CreatedAt).ToList());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var project = await _projectService.GetProjectByIdAsync(id.Value);
                
            if (project == null) return NotFound();

            return View(project);
        }
    }
}
