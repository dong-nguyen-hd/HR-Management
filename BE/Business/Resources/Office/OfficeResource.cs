using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Office
{
    public class OfficeResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
    }
}
