using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SessionId { get; set; } = string.Empty;

        [Required]
        public int QuestionId { get; set; }

        public string? ResponseText { get; set; }

        public DateTime SubmittedAt { get; set; }

        // Navigation properties
        [ForeignKey("SessionId")]
        public virtual ExamSession? Session { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
    }
}