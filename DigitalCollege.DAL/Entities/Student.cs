using System.Collections.Generic;

namespace DigitalCollege.DAL.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}