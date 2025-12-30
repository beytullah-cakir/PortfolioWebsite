using System.Text.Json;
using Portfolio.Models;

namespace Portfolio.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task SaveProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task UpdateProjectAsync(Project project);
    }

    public class ProjectService : IProjectService
    {
        private readonly string _jsonPath;

        public ProjectService(IWebHostEnvironment env)
        {
            _jsonPath = Path.Combine(env.WebRootPath, "data", "projects.json");
            
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_jsonPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
            
            // Ensure file exists
            if (!File.Exists(_jsonPath))
            {
                File.WriteAllText(_jsonPath, "[]");
            }
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            var json = await File.ReadAllTextAsync(_jsonPath);
            return JsonSerializer.Deserialize<List<Project>>(json) ?? new List<Project>();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            var projects = await GetAllProjectsAsync();
            return projects.FirstOrDefault(p => p.Id == id);
        }

        public async Task SaveProjectAsync(Project project)
        {
            var projects = await GetAllProjectsAsync();
            project.Id = projects.Any() ? projects.Max(p => p.Id) + 1 : 1;
            project.CreatedAt = DateTime.UtcNow;
            projects.Add(project);
            await WriteToJsonAsync(projects);
        }

        public async Task UpdateProjectAsync(Project project)
        {
            var projects = await GetAllProjectsAsync();
            var index = projects.FindIndex(p => p.Id == project.Id);
            if (index != -1)
            {
                projects[index] = project;
                await WriteToJsonAsync(projects);
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            var projects = await GetAllProjectsAsync();
            var project = projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                projects.Remove(project);
                await WriteToJsonAsync(projects);
            }
        }

        private async Task WriteToJsonAsync(List<Project> projects)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(projects, options);
            await File.WriteAllTextAsync(_jsonPath, json);
        }
    }
}
