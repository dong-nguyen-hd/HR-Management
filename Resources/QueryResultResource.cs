using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources
{
    public class QueryResultResource<T>
    {
        [Display(Name = "Total Items")]
        public int TotalItems { get; set; } = 0;
        public List<T> Items { get; set; } = new List<T>();
    }
}
