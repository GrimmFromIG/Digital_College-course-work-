namespace DigitalCollege.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public int? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public int? StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}