using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Technology
{
    public class UpdateTechnologyResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
