using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Data
{
    public class DatabaseInitializer
    {
        private readonly ExamDbContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(
            ExamDbContext context,
            ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Initialize()
        {
            try
            {
                // Create database if it doesn't exist
                await _context.Database.EnsureCreatedAsync();

                // Check if we already have questions
                if (!await _context.Questions.AnyAsync())
                {
                    await SeedQuestionsAsync();
                    _logger.LogInformation("Seeded the database with questions");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }

        private async Task SeedQuestionsAsync()
        {
            // Add some initial questions
            var questions = new List<Question>
            {
                // Personal Information section is handled separately
                
                // General Knowledge Section
                new Question { 
                    Text = "What is the capital of France?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 1,
                    CorrectAnswer = "Paris"
                },
                new Question { 
                    Text = "What is the value of pi (up to 2 decimal places)?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 2,
                    CorrectAnswer = "3.14"
                },
                new Question { 
                    Text = "Who wrote 'Romeo and Juliet'?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 3,
                    CorrectAnswer = "William Shakespeare"
                },
                new Question { 
                    Text = "Which planet is known as the Red Planet?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 4,
                    CorrectAnswer = "Mars"
                },
                new Question { 
                    Text = "What is the chemical symbol for gold?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "General Knowledge", 
                    DisplayOrder = 5,
                    CorrectAnswer = "Au"
                },
                
                // Mathematics Section
                new Question { 
                    Text = "Solve for x: 2x + 5 = 13", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Mathematics", 
                    DisplayOrder = 1,
                    CorrectAnswer = "4"
                },
                new Question { 
                    Text = "What is the area of a circle with radius 4 cm?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Mathematics", 
                    DisplayOrder = 2,
                    CorrectAnswer = "50.27"
                },
                new Question { 
                    Text = "If a = 3 and b = 4, what is the value of a² + b²?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Mathematics", 
                    DisplayOrder = 3,
                    CorrectAnswer = "25"
                },
                
                // Science Section
                new Question { 
                    Text = "What is the chemical formula for water?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Science", 
                    DisplayOrder = 1,
                    CorrectAnswer = "H2O"
                },
                new Question { 
                    Text = "What is the process by which plants make their food?", 
                    Type = QuestionType.MultipleChoice, 
                    SectionName = "Science", 
                    DisplayOrder = 2,
                    Options = "Photosynthesis|Respiration|Digestion|Transpiration",
                    CorrectAnswer = "Photosynthesis"
                },
                new Question { 
                    Text = "What is the unit of force in the International System of Units?", 
                    Type = QuestionType.ShortAnswer, 
                    SectionName = "Science", 
                    DisplayOrder = 3,
                    CorrectAnswer = "Newton"
                },
                
                // Essay Question
                new Question { 
                    Text = "Explain the importance of environmental conservation in today's world (minimum 100 words)", 
                    Type = QuestionType.Essay, 
                    SectionName = "Essay", 
                    DisplayOrder = 1
                }
            };

            await _context.Questions.AddRangeAsync(questions);
            await _context.SaveChangesAsync();
        }
    }
}