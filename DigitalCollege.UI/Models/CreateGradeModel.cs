using System.ComponentModel.DataAnnotations;

namespace DigitalCollege.UI.Models
{
    public class CreateGradeModel
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Оберіть студента зі списку!")]
        public int StudentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Оберіть дисципліну зі списку!")]
        public int DisciplineId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Вкажіть бал!")]
        [Range(0, 100, ErrorMessage = "Оцінка має бути від 0 до 100 балів!")]
        public int Value { get; set; }
    }
}
