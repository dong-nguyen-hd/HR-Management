using System.ComponentModel.DataAnnotations;

namespace Business.Resources.Pay
{
    public class PayResource
    {
        public int Id { get; set; }
        public decimal NET { get; set; }
        public decimal Gross { get; set; }
        public decimal Salary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Bonus { get; set; }

        [Display(Name = "Personal Income Tax")]
        public decimal PIT { get; set; }

        [Display(Name = "Social Insurance")]
        public decimal SocialInsurance { get; set; }

        [Display(Name = "Health Insurance")]
        public decimal HealthInsurance { get; set; }
        public DateTime Date { get; set; }
    }
}
