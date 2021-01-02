using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Certificate
{
    public class CreateCertificateResource
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Provider { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}
