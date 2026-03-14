namespace PosSystem.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = "System";

        public string UpdatedBy { get; set; } = "System";
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}