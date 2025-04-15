using System.Threading.Tasks;
using Exam.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Exam.Pages
{
    public class ExamSuccessModel : PageModel
    {
        private readonly ExamDbContext _context;

        public ExamSuccessModel(ExamDbContext context)
        {
            _context = context;
        }

        public string FullName { get; set; } = string.Empty;
        
        public string SessionId { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToPage("/Index");
            }

            var session = await _context.ExamSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null)
            {
                return NotFound();
            }

            FullName = session.FullName;
            SessionId = session.SessionId;

            return Page();
        }
    }
}