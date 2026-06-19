using System;
namespace JobJump.Models
{
    public class JobApplication
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public string? UserId { get; set; }

    public string? ResumePath { get; set; }

    public DateTime AppliedOn { get; set; }

    public string? Status { get; set; }

    public int MatchScore { get; set; }

    public string? MissingSkills { get; set; }

    public Job? Job { get; set; }

    public ApplicationUser? User { get; set; }
}
}

