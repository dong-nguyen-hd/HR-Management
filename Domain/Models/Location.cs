using System.Collections.Generic;

namespace HR_Management.Domain.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public bool Status { get; set; }
        public ICollection<Person> People { get; set; }
    }
}
