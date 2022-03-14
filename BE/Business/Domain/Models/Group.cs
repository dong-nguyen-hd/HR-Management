using System;
using System.Collections.Generic;

namespace Business.Domain.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeamSize { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Technology { get; set; }
        public bool Status { get; set; }
        public HashSet<Project> Projects { get; set; }
    }
}
