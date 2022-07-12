namespace Business.Domain.Models
{
    public class Pay
    {
        public int Id { get; set; }
        public decimal Salary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Bonus { get; set; }
        public decimal PIT { get; set; }
        public decimal SocialInsurance { get; set; }
        public decimal HealthInsurance { get; set; }
        public DateTime Date { get; set; }
        public bool IsDeleted { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
