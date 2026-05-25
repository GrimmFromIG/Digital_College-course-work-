using System.ComponentModel.DataAnnotations;

namespace DigitalCollege.UI.Models
{
    public class CreateGradeModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Будь ласка, оберіть студента зі списку!")]
        public int StudentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Будь ласка, оберіть дисципліну зі списку!")]
        public int DisciplineId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Вкажіть бал!")]
        [Range(0, 100, ErrorMessage = "Оцінка не може бути меншою за 0 або більшою за 100 балів!")]
        public int Value { get; set; }
    }
}