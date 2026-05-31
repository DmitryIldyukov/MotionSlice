namespace Auth.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; init; } = Guid.CreateVersion7();
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}