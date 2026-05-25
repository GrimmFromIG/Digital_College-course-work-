namespace DigitalCollege.UI.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public int? TeacherId { get; set; }
        public int? StudentId { get; set; }
        public string FullName { get; set; }
    }
}