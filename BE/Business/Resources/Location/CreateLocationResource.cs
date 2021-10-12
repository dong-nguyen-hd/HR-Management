using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Location
{
    public class CreateLocationResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
    }
}
