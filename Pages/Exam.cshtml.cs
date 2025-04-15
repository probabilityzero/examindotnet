using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Data;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages
{
    public class ExamModel : PageModel
    {
        private readonly ExamDbContext _context;

        public ExamModel(ExamDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ExamSession Session { get; set; } = new();

        [BindProperty]
        public Dictionary<int, string> Responses { get; set; } = new();

        public List<Question> CurrentQuestions { get; set; } = new();
        
        public List<string> AvailableSections { get; set; } = new();
        
        public string CurrentSectionName { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string? sessionId = null)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                // New exam
                Session = new ExamSession
                {
                    StartTime = DateTime.Now,
                    CurrentSection = 0 // Personal information
                };
                
                return Page();
            }
            else
            {
                // Continue existing exam
                var existingSession = await _context.ExamSessions
                    .Include(s => s.Answers)
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);
                
                if (existingSession == null)
                {
                    // Session not found
                    return RedirectToPage("/Error");
                }
                
                Session = existingSession;
                
                // Populate responses from existing answers if we're not on the personal info section
                if (Session.CurrentSection > 0)
                {
                    await LoadCurrentSectionQuestions();
                    
                    foreach (var answer in Session.Answers.Where(a => CurrentQuestions.Any(q => q.Id == a.QuestionId)))
                    {
                        Responses[answer.QuestionId] = answer.ResponseText ?? string.Empty;
                    }
                }
                
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "next" || action == "submit")
            {
                if (Session.CurrentSection == 0)
                {
                    // Validate personal information
                    if (string.IsNullOrWhiteSpace(Session.FullName) ||
                        string.IsNullOrWhiteSpace(Session.Email) ||
                        string.IsNullOrWhiteSpace(Session.RollNumber))
                    {
                        ModelState.AddModelError(string.Empty, "Please fill out all personal information fields.");
                        return Page();
                    }

                    // Save session for the first time
                    if (string.IsNullOrEmpty(Session.SessionId))
                    {
                        Session.SessionId = Guid.NewGuid().ToString();
                    }
                    
                    _context.ExamSessions.Add(Session);
                    await _context.SaveChangesAsync();
                    
                    // Move to the first question section
                    Session.CurrentSection = 1;
                }
                else
                {
                    // Load questions for the current section
                    await LoadCurrentSectionQuestions();
                    
                    // Validate responses
                    bool isValid = true;
                    foreach (var question in CurrentQuestions)
                    {
                        if (!Responses.ContainsKey(question.Id) || string.IsNullOrWhiteSpace(Responses[question.Id]))
                        {
                            ModelState.AddModelError(string.Empty, $"Please answer all questions in this section.");
                            isValid = false;
                            break;
                        }
                    }
                    
                    if (!isValid)
                    {
                        return Page();
                    }
                    
                    // Save responses
                    foreach (var question in CurrentQuestions)
                    {
                        var existingAnswer = await _context.Answers
                            .FirstOrDefaultAsync(a => a.SessionId == Session.SessionId && a.QuestionId == question.Id);
                            
                        if (existingAnswer != null)
                        {
                            // Update existing answer
                            existingAnswer.ResponseText = Responses[question.Id];
                            existingAnswer.SubmittedAt = DateTime.Now;
                        }
                        else
                        {
                            // Create new answer
                            var answer = new Answer
                            {
                                SessionId = Session.SessionId,
                                QuestionId = question.Id,
                                ResponseText = Responses[question.Id],
                                SubmittedAt = DateTime.Now
                            };
                            _context.Answers.Add(answer);
                        }
                    }
                    
                    // If submitting the final section
                    if (action == "submit")
                    {
                        Session.EndTime = DateTime.Now;
                        _context.Update(Session);
                        await _context.SaveChangesAsync();
                        
                        return RedirectToPage("/ExamSuccess", new { sessionId = Session.SessionId });
                    }
                    
                    // Prepare for next section
                    Session.CurrentSection++;
                    _context.Update(Session);
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToPage("/Exam", new { sessionId = Session.SessionId });
            }
            
            return Page();
        }
        
        private async Task LoadCurrentSectionQuestions()
        {
            // Get all available sections
            AvailableSections = await _context.Questions
                .Select(q => q.SectionName)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();
                
            if (Session.CurrentSection <= 0 || Session.CurrentSection > AvailableSections.Count)
            {
                CurrentQuestions = new List<Question>();
                return;
            }
            
            // Get section name based on current section index (1-based)
            CurrentSectionName = AvailableSections[Session.CurrentSection - 1];
            
            // Get questions for the current section
            CurrentQuestions = await _context.Questions
                .Where(q => q.SectionName == CurrentSectionName)
                .OrderBy(q => q.DisplayOrder)
                .ToListAsync();
        }
    }
}