using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsFinally { get; set; }

        // Navigation props
        [ForeignKey(nameof(UserId))]
        public AppUser AppUser { get; set; }
    }
}
