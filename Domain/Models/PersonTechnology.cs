using System.Collections.Generic;

namespace HR_Management.Domain.Models
{
    public class PersonTechnology
    {
        public ICollection<Person> PersonId { get; set; }
        public ICollection<Technology> TechnologyId { get; set; }
    }
}
