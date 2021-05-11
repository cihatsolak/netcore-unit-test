namespace UnitTest.Web.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public int SchoolId { get; set; }
        public virtual School School { get; set; }
    }
}
