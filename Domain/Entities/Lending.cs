namespace Domain.Entities
{
    public class Lending
    {
        public Guid Id { get; set; }
        public DateTime LendedAt { get; set; }
        public DateTime ReturnAt { get; set; }
        public bool IsBeingReturned { get; set; }

        // Navigation props
        public Book Book { get; set; }
        public AppUser AppUser { get; set; }
    }
}
