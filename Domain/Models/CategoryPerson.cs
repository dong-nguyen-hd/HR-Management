namespace HR_Management.Domain.Models
{
    public class CategoryPerson
    {
        public int Id { get; set; }
        public int OrderIndex { get; set; }
        public string Technology { get; set; }
        public bool Status { get; set; }
        public int CategoryId { get; set; }
        public int PersonId { get; set; }
        public Category Category { get; set; }
        public  Person Person { get; set; }
    }
}
