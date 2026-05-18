using System.ComponentModel.DataAnnotations;

namespace DigitalCollege.UI.Models
{
    public class GroupModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва групи обов'язкова для заповнення!")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Назва групи має бути від 2 до 20 символів.")]
        public string Name { get; set; }
    }
}
