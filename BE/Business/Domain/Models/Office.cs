using System.Collections.Generic;

namespace Business.Domain.Models
{
    public class Office
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public HashSet<Person> People { get; set; }
    }
}
