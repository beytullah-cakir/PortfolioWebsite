using System.Collections.Generic;

namespace Portfolio.Models
{
    public class AdminViewModel
    {
        public IEnumerable<Project> Projects { get; set; } = new List<Project>();
    }
}
