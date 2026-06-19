using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JobJump.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobJump.Controllers
{
    [Authorize(Roles = "Employer")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // TOTAL JOBS
            var totalJobs = await _context.Jobs
                .CountAsync(j => j.EmployerId == userId);

            // GET EMPLOYER JOB IDS
            var employerJobIds = await _context.Jobs
                .Where(j => j.EmployerId == userId)
                .Select(j => j.Id)
                .ToListAsync();

            // TOTAL APPLICATIONS
            var totalApplications = await _context.JobApplications
                .CountAsync(a => employerJobIds.Contains(a.JobId));

            // ACCEPTED
            var accepted = await _context.JobApplications
                .CountAsync(a =>
                    employerJobIds.Contains(a.JobId) &&
                    a.Status == "Accepted");

            // REJECTED
            var rejected = await _context.JobApplications
                .CountAsync(a =>
                    employerJobIds.Contains(a.JobId) &&
                    a.Status == "Rejected");

            // PENDING
            var pending = await _context.JobApplications
                .CountAsync(a =>
                    employerJobIds.Contains(a.JobId) &&
                    a.Status == "Pending");

            ViewBag.TotalJobs = totalJobs;
            ViewBag.TotalApplications = totalApplications;
            ViewBag.Accepted = accepted;
            ViewBag.Rejected = rejected;
            ViewBag.Pending = pending;

            return View();
        }
    }
}