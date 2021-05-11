using System.Collections.Generic;

namespace UnitTest.Web.Models
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
