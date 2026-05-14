using System;

namespace DigitalCollege.DAL.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime DateAssigned { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int DisciplineId { get; set; }
        public virtual Discipline Discipline { get; set; }
    }
}