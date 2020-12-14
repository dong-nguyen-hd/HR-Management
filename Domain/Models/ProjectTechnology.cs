using System.Collections.Generic;

namespace HR_Management.Domain.Models
{
    public class ProjectTechnology
    {
        public ICollection<Project> ProjectId { get; set; }
        public ICollection<Technology> TechnologyId { get; set; }
    }
}
