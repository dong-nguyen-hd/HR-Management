using HR_Management.Extensions;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace HR_Management.Resources.Certificate
{
    public class CertificateResource
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string Provider { get; set; }

        [Required]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartDate { get; set; }

        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime? EndDate { get; set; }

        [Required]
        public int OrderIndex { get; set; }
    }
}
