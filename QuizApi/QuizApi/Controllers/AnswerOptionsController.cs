using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Data;
using QuizApi.DTOs;
using QuizApi.Models;

namespace QuizApi.Controllers
{
    [ApiController]
    [Route("api/questions/{questionId}/options")]
    public class AnswerOptionsController : ControllerBase
    {
        private readonly QuizDbContext _context;
        public AnswerOptionsController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AnswerOptionDto>>> GetAll([FromRoute] int questionId)
        {
            var questionExists = await _context.Questions.AnyAsync(q => q.Id == questionId);
            if (!questionExists)
            {
                return NotFound("Вопрос не найден");
            }

            var answerOptions = await _context.AnswerOptions.Where(a => a.QuestionId == questionId).Select(a => new AnswerOptionDto
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect,
                QuestionId = questionId
            }).ToListAsync();

            return Ok(answerOptions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AnswerOptionDto>> GetById(int id, [FromRoute] int questionId)
        {
            var answerOption = await _context.AnswerOptions.Where(a => a.Id == id && a.QuestionId == questionId).Select(a => new AnswerOptionDto
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect,
                QuestionId = a.QuestionId
            }).FirstOrDefaultAsync();

            if (answerOption == null)
            {
                return NotFound("Вариант ответа не найден");
            }

            return Ok(answerOption);
        }

        [HttpPost]
        public async Task<ActionResult<AnswerOptionDto>> Create(CreateAnswerOptionDto dto, [FromRoute] int questionId)
        {
            var questionExists = await _context.Questions.AnyAsync(q => q.Id == questionId);
            if (!questionExists)
            {
                return NotFound("Вопрос не найден");
            }

            if(dto.IsCorrect)
            {
                var existingCorrectOption = await _context.AnswerOptions.FirstOrDefaultAsync(a => a.QuestionId == questionId && a.IsCorrect);
                if (existingCorrectOption != null)
                {
                    existingCorrectOption.IsCorrect = false;
                }
            }

            var answerOption = new AnswerOption
            {
                Text = dto.Text,
                IsCorrect = dto.IsCorrect,
                QuestionId = questionId
            };
            _context.AnswerOptions.Add(answerOption);
            await _context.SaveChangesAsync();

            var answerOptionDto = new AnswerOptionDto
            {
                Id = answerOption.Id,
                Text = answerOption.Text,
                IsCorrect = answerOption.IsCorrect,
                QuestionId = answerOption.QuestionId
            };

            return CreatedAtAction(nameof(GetById), new { id = answerOption.Id, questionId }, answerOptionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAnswerOptionDto dto, [FromRoute] int questionId)
        {
            var answerOption = await _context.AnswerOptions.FirstOrDefaultAsync(a => a.Id == id && a.QuestionId == questionId);
            if (answerOption == null)
            {
                return NotFound("Вариант ответа не найден");
            }

            if (dto.IsCorrect)
            {
                var existingCorrectOption = await _context.AnswerOptions.FirstOrDefaultAsync(a => a.QuestionId == questionId && a.IsCorrect && a.Id != id);
                if (existingCorrectOption != null)
                {
                    existingCorrectOption.IsCorrect = false;
                }
            }

            answerOption.Text = dto.Text;
            answerOption.IsCorrect = dto.IsCorrect;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromRoute] int questionId)
        {
            var answerOption = await _context.AnswerOptions.FirstOrDefaultAsync(a => a.Id == id && a.QuestionId == questionId);
            if (answerOption == null)
            {
                return NotFound("Вариант ответа не найден");
            }

            _context.AnswerOptions.Remove(answerOption);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
