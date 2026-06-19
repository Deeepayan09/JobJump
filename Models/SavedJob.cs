using System.ComponentModel.DataAnnotations.Schema;

namespace JobJump.Models
{
    public class SavedJob
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        public Job? Job { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}