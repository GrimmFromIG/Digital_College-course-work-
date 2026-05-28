namespace DigitalCollege.PL.ViewModels
{
    public class CreateGradeRequest
    {
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public int DisciplineId { get; set; }
        public int Value { get; set; }
    }
}