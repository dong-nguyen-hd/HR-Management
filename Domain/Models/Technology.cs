using System.Collections.Generic;

namespace HR_Management.Domain.Models
{
    public class Technology
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public bool Status { get; set; }
        public Category Category { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<Person> People { get; set; }
    }
}
