using System.IO;

namespace JobJump.Services
{
    public class ResumeAIService
    {
        public int CalculateResumeScore(string? resumePath)
        {
            if (string.IsNullOrEmpty(resumePath))
            {
                return 0;
            }

            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                resumePath.TrimStart('/')
            );

            if (!File.Exists(fullPath))
            {
                return 0;
            }

            var extension = Path.GetExtension(fullPath).ToLower();

            // SIMPLE AI SCORE
            int score = 50;

            // BONUS FOR PDF
            if (extension == ".pdf")
            {
                score += 20;
            }

            // BONUS FOR LARGE RESUME
            var fileInfo = new FileInfo(fullPath);

            if (fileInfo.Length > 100000)
            {
                score += 20;
            }

            // MAX 100
            if (score > 100)
            {
                score = 100;
            }

            return score;
        }
    }
}