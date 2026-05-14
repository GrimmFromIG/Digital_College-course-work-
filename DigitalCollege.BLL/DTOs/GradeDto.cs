using System;

namespace DigitalCollege.BLL.DTOs
{
    public class GradeDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime DateAssigned { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public int TeacherId { get; set; }
    }
}