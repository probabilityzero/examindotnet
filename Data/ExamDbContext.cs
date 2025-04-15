using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Data
{
    public class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions<ExamDbContext> options) : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<ExamSession> ExamSessions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Session)
                .WithMany(s => s.Answers)
                .HasForeignKey(a => a.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId);

            // Seed some initial questions
            modelBuilder.Entity<Question>().HasData(
                // Personal Information section is handled separately
                
                // General Knowledge Section
                new Question { 
                    Id = 1, 
                    Text = "What is the capital of France?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 1,
                    CorrectAnswer = "Paris"
                },
                new Question { 
                    Id = 2, 
                    Text = "What is the value of pi (up to 2 decimal places)?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 2,
                    CorrectAnswer = "3.14"
                },
                new Question { 
                    Id = 3, 
                    Text = "Who wrote 'Romeo and Juliet'?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 3,
                    CorrectAnswer = "William Shakespeare"
                },
                new Question { 
                    Id = 4, 
                    Text = "Which planet is known as the Red Planet?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 4,
                    CorrectAnswer = "Mars"
                },
                new Question { 
                    Id = 5, 
                    Text = "What is the chemical symbol for gold?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 5,
                    CorrectAnswer = "Au"
                },
                
                // Mathematics Section
                new Question { 
                    Id = 6, 
                    Text = "Solve for x: 2x + 5 = 13", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Mathematics", 
                    DisplayOrder = 1,
                    CorrectAnswer = "4"
                },
                new Question { 
                    Id = 7, 
                    Text = "What is the area of a circle with radius 4 cm?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Mathematics", 
                    DisplayOrder = 2,
                    CorrectAnswer = "50.27"
                },
                new Question { 
                    Id = 8, 
                    Text = "If a = 3 and b = 4, what is the value of a² + b²?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Mathematics", 
                    DisplayOrder = 3,
                    CorrectAnswer = "25"
                },
                
                // Science Section
                new Question { 
                    Id = 9, 
                    Text = "What is the chemical formula for water?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Science", 
                    DisplayOrder = 1,
                    CorrectAnswer = "H2O"
                },
                new Question { 
                    Id = 10, 
                    Text = "What is the process by which plants make their food?", 
                    Type = QuestionType.MultipleChoice, 
                    SectionName = "Science", 
                    DisplayOrder = 2,
                    Options = "Photosynthesis|Respiration|Digestion|Transpiration",
                    CorrectAnswer = "Photosynthesis"
                },
                new Question { 
                    Id = 11, 
                    Text = "What is the unit of force in the International System of Units?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Science", 
                    DisplayOrder = 3,
                    CorrectAnswer = "Newton"
                },
                
                // Essay Question
                new Question { 
                    Id = 12, 
                    Text = "Explain the importance of environmental conservation in today's world (minimum 100 words)", 
                    Type = QuestionType.Essay, 
                    SectionName = "Essay", 
                    DisplayOrder = 1
                }
            );
        }
    }
}