using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Data;
using QuizApi.DTOs;
using QuizApi.Models;

namespace QuizApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizzesController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public QuizzesController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizDto>>> GetAll([FromQuery] int? themeId)
        {
            var query = _context.Quizzes.AsQueryable();

            if (themeId.HasValue)
            {
                query = query.Where(q => q.ThemeId == themeId.Value);
            }

            var quizzes = await query.Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                ThemeId = q.ThemeId
            }).ToListAsync();
            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetById(int id)
        {
            var quiz = await _context.Quizzes.Where(q => q.Id == id).Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                ThemeId = q.ThemeId
            }).FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(quiz);
            }
        }

        [HttpPost]
        public async Task<ActionResult<QuizDto>> Create(CreateQuizDto dto)
        {
            var theme = await _context.Themes.FindAsync(dto.ThemeId);
            if (theme == null)
            {
                return BadRequest($"Тема с Id = {dto.ThemeId} не найдена");
            }


            Quiz quiz = new Quiz { Title = dto.Title, ThemeId = dto.ThemeId };
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            var quizDto = new QuizDto
            {
                Id = quiz.Id,
                Title = quiz.Title,
                ThemeId = quiz.ThemeId
            };
            return CreatedAtAction(nameof(GetById), new { id = quiz.Id }, quizDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateQuizDto dto)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes.FindAsync(dto.ThemeId);
            if (theme == null)
            {
                return BadRequest($"Тема с Id = {dto.ThemeId} не найдена");
            }

            quiz.Title = dto.Title;
            quiz.ThemeId = dto.ThemeId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
