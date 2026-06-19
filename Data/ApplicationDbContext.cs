using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JobJump.Models;

namespace JobJump.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<SavedJob> SavedJobs { get; set; }

        
    }
}