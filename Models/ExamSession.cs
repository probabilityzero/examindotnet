using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class ExamSession
    {
        [Key]
        public string SessionId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string RollNumber { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int CurrentSection { get; set; } = 0;

        // Navigation property
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}