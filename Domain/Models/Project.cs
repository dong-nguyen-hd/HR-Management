using System;
using System.Collections.Generic;

namespace HR_Management.Domain.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public string Responsibilities { get; set; }
        public short TeamSize { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte OrderIndex { get; set; }
        public bool Status { get; set; }
        public int PersonId { get; set; }
        public ICollection<Person> People { get; set; }
        public ICollection<Technology> Technologies { get; set; }
    }
}
