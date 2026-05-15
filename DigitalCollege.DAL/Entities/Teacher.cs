namespace DigitalCollege.DAL.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public ICollection<Discipline> Disciplines { get; set; }
        public User User { get; set; }
    }
}
