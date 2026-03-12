namespace PosSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }

        public DateTime lastLogin { get; set; }
    }
}