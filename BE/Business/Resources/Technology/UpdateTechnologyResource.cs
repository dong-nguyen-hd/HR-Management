using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Technology
{
    public class UpdateTechnologyResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
