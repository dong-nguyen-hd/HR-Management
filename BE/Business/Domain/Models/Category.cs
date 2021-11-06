using System.Collections.Generic;

namespace Business.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public CategoryPerson CategoryPerson { get; set; }
        public HashSet<Technology> Technologies { get; set; }
    }
}
