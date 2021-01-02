using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Technology
{
    public class TechnologyResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
