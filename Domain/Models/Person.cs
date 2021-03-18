using System;
using System.Collections.Generic;

namespace HR_Management.Domain.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string StaffId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public DateTime YearOfBirth { get; set; }
        public byte Gender { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
        public string OrderIndex { get; set; }
        public int? LocationId { get; set; }
        public Location Location { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
        public ICollection<CategoryPerson> CategoryPersons { get; set; }
        public ICollection<Education> Educations { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<WorkHistory> WorkHistories { get; set; }
        public ICollection<Log> Logs { get; set; }
    }
}
