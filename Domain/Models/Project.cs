using System;

namespace HR_Management.Domain.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public string Responsibilities { get; set; }
        public int TeamSize { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int OrderIndex { get; set; }
        public bool Status { get; set; }
        public string Technology { get; set; }
        public int PersonId { get; set; }
        public Person People { get; set; }
    }
}
