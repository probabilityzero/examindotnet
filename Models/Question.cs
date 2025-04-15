using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public QuestionType Type { get; set; }

        [Required]
        public string SectionName { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }

        // For multiple choice questions
        public string? Options { get; set; }

        // For verification/grading (not shown to users)
        public string? CorrectAnswer { get; set; }

        [NotMapped]
        public List<string> OptionsList 
        {
            get => Options?.Split('|').ToList() ?? new List<string>();
        }
    }

    public enum QuestionType
    {
        ShortAnswer,
        MultipleChoice,
        Essay
    }
}