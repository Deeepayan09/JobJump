using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JobJump.Data;
using JobJump.Models;
using JobJump.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobJump.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ResumeAIService _resumeAIService;
        private readonly EmailService _emailService;

        public JobController(
            ApplicationDbContext context,
            ResumeAIService resumeAIService,
            EmailService emailService)
        {
            _context = context;
            _resumeAIService = resumeAIService;
            _emailService = emailService;
        }

        // =========================
        // VIEW ALL JOBS + SEARCH
        // =========================
        [AllowAnonymous]
        public async Task<IActionResult> Index(string? search, string? company)
        {
            var jobs = _context.Jobs.AsQueryable();

            // SEARCH BY TITLE
            if (!string.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(j => j.Title.Contains(search));
            }

            // FILTER BY COMPANY
            if (!string.IsNullOrEmpty(company))
            {
                jobs = jobs.Where(j => j.Company.Contains(company));
            }

            var jobList = await jobs.ToListAsync();

            // SMART APPLY
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var appliedJobs = await _context.JobApplications
                    .Where(a => a.UserId == userId)
                    .Select(a => a.JobId)
                    .ToListAsync();

                ViewBag.AppliedJobs = appliedJobs;
            }

            return View(jobList);
        }

        // =========================
        // JOB DETAILS
        // =========================
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var job = await _context.Jobs
                .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // =========================
        // EMPLOYER DASHBOARD
        // =========================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == userId)
                .ToListAsync();

            var jobIds = jobs.Select(j => j.Id).ToList();

            var applications = await _context.JobApplications
                .Where(a => jobIds.Contains(a.JobId))
                .ToListAsync();

            var accepted = applications.Count(a => a.Status == "Accepted");
            var rejected = applications.Count(a => a.Status == "Rejected");
            var pending = applications.Count(a => a.Status == "Pending");

            ViewBag.TotalJobs = jobs.Count;
            ViewBag.TotalApplications = applications.Count;
            ViewBag.Accepted = accepted;
            ViewBag.Rejected = rejected;
            ViewBag.Pending = pending;

            ViewBag.ChartLabels = new[]
            {
                "Accepted",
                "Rejected",
                "Pending"
            };

            ViewBag.ChartData = new[]
            {
                accepted,
                rejected,
                pending
            };

            return View();
        }

        // =========================
        // EMPLOYER: MY JOBS
        // =========================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> MyJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == userId)
                .ToListAsync();

            return View(jobs);
        }

        // =========================
        // CREATE JOB
        // =========================
        [Authorize(Roles = "Employer")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                job.EmployerId = userId;

                _context.Jobs.Add(job);

                await _context.SaveChangesAsync();

                TempData["Message"] = "Job created successfully!";

                return RedirectToAction(nameof(MyJobs));
            }

            return View(job);
        }

        // =========================
        // EDIT JOB
        // =========================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Edit(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (job.EmployerId != userId)
            {
                return Unauthorized();
            }

            return View(job);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Job job)
        {
            if (id != job.Id)
            {
                return NotFound();
            }

            var existingJob = await _context.Jobs
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.Id == id);

            if (existingJob == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (existingJob.EmployerId != userId)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                job.EmployerId = userId;

                _context.Update(job);

                await _context.SaveChangesAsync();

                TempData["Message"] = "Job updated successfully!";

                return RedirectToAction(nameof(MyJobs));
            }

            return View(job);
        }

        // =========================
        // DELETE JOB
        // =========================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        [Authorize(Roles = "Employer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);

            if (job != null)
            {
                _context.Jobs.Remove(job);

                await _context.SaveChangesAsync();
            }

            TempData["Message"] = "Job deleted successfully!";

            return RedirectToAction(nameof(MyJobs));
        }

        // =========================
        // APPLY FOR JOB
        // =========================
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int id, IFormFile? resume)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            // CHECK DUPLICATE
            var alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.JobId == id && a.UserId == userId);

            if (alreadyApplied)
            {
                TempData["Message"] = "You already applied for this job.";

                return RedirectToAction(nameof(Index));
            }

            string? filePath = null;

            // RESUME UPLOAD
            if (resume != null && resume.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString()
                               + Path.GetExtension(resume.FileName);

                var uploadFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "resumes"
                );

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var fullPath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await resume.CopyToAsync(stream);
                }

                filePath = "/resumes/" + fileName;
            }

            // CREATE APPLICATION
            var application = new JobApplication
            {
                JobId = id,
                UserId = userId,
                ResumePath = filePath,
                AppliedOn = DateTime.Now,
                Status = "Pending"
            };

            _context.JobApplications.Add(application);

            await _context.SaveChangesAsync();

            // SEND EMAIL TO EMPLOYEE
            var user = await _context.Users.FindAsync(userId);

            var job = await _context.Jobs.FindAsync(id);

            if (user != null && job != null)
            {
                await _emailService.SendEmailAsync(
                    user.Email!,
                    "Application Submitted Successfully",
                    $@"
                        <h2>Application Submitted 🚀</h2>

                        <p>
                            You successfully applied for
                            <b>{job.Title}</b>
                            at
                            <b>{job.Company}</b>.
                        </p>

                        <p>
                            Status: <b>Pending</b>
                        </p>

                        <br>

                        <p>
                            Thank you for using JobJump ❤️
                        </p>
                    "
                );
            }

            TempData["Message"] = "Application submitted successfully!";

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // SAVE JOB
        // =========================
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Save(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var alreadySaved = await _context.SavedJobs
                .AnyAsync(s => s.JobId == id && s.UserId == userId);

            if (!alreadySaved)
            {
                var savedJob = new SavedJob
                {
                    JobId = id,
                    UserId = userId
                };

                _context.SavedJobs.Add(savedJob);

                await _context.SaveChangesAsync();

                TempData["Message"] = "Job saved successfully!";
            }
            else
            {
                TempData["Message"] = "Job already saved!";
            }

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // VIEW SAVED JOBS
        // =========================
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> SavedJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var savedJobs = await _context.SavedJobs
                .Include(s => s.Job)
                .Where(s => s.UserId == userId)
                .ToListAsync();

            return View(savedJobs);
        }

        // =========================
        // MY APPLICATIONS
        // =========================
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> MyApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return View(applications);
        }

        // =========================
        // VIEW APPLICANTS + AI SCORE
        // =========================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Applicants(int jobId)
        {
            var applications = await _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Job)
                .Where(a => a.JobId == jobId)
                .ToListAsync();

            // AI SCORE
            foreach (var app in applications)
            {
                app.MatchScore =
                    _resumeAIService.CalculateResumeScore(app.ResumePath);
            }

            return View(applications);
        }

        // =========================
        // ACCEPT APPLICATION
        // =========================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Accept(int id)
        {
            var application = await _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application != null)
            {
                application.Status = "Accepted";

                await _context.SaveChangesAsync();

                // SEND EMAIL
                await _emailService.SendEmailAsync(
                    application.User.Email!,
                    "Application Accepted 🎉",
                    $@"
                        <h2>Congratulations 🎉</h2>

                        <p>
                            Your application for
                            <b>{application.Job.Title}</b>
                            at
                            <b>{application.Job.Company}</b>
                            has been accepted.
                        </p>

                        <p>
                            The employer may contact you soon.
                        </p>

                        <br>

                        <p>
                            Team JobJump 🚀
                        </p>
                    "
                );
            }

            TempData["Message"] = "Application accepted!";

            return RedirectToAction(nameof(MyJobs));
        }

        // =========================
        // REJECT APPLICATION
        // =========================
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Reject(int id)
        {
            var application = await _context.JobApplications
                .Include(a => a.User)
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application != null)
            {
                application.Status = "Rejected";

                await _context.SaveChangesAsync();

                // SEND EMAIL
                await _emailService.SendEmailAsync(
                    application.User.Email!,
                    "Application Update",
                    $@"
                        <h2>Application Update</h2>

                        <p>
                            Your application for
                            <b>{application.Job.Title}</b>
                            at
                            <b>{application.Job.Company}</b>
                            was not selected.
                        </p>

                        <p>
                            Keep applying and improving 🚀
                        </p>

                        <br>

                        <p>
                            Team JobJump
                        </p>
                    "
                );
            }

            TempData["Message"] = "Application rejected!";

            return RedirectToAction(nameof(MyJobs));
        }
    }
}