
namespace DigitalCollege.UI.Models
{
    public class GradeModel
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime DateAssigned { get; set; }
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public string StudentName { get; set; }
        public string TeacherName { get; set; }
    }
}
