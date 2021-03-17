using System;

namespace HR_Management.Domain.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Perpose { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
