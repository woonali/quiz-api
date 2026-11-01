using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Data;
using QuizApi.DTOs;
using QuizApi.Models;

namespace QuizApi.Controllers
{
    [ApiController]
    [Route("api/quizzes/{quizId}/[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly QuizDbContext _context;
        public QuestionsController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetAll([FromRoute] int quizId)
        {
            var quizExists = await _context.Quizzes.AnyAsync(q => q.Id == quizId);

            if (!quizExists)
            {
                return NotFound("Викторина не найдена");
            }

            var questions = await _context.Questions.Where(q => q.QuizId == quizId).Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Points = q.Points,
                QuizId = quizId
            }).ToListAsync();

            return Ok(questions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuestionDto>> GetById(int id, [FromRoute] int quizId)
        {
            var question = await _context.Questions.Where(q => q.Id == id && q.QuizId == quizId).Select(q => new QuestionDto
            {
                Id = q.Id,
                Text = q.Text,
                Points = q.Points,
                QuizId = q.QuizId
            }).FirstOrDefaultAsync();

            if(question == null)
            {
                return NotFound("Вопрос не найден");
            }

            return Ok(question);
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Create(CreateQuestionDto dto, [FromRoute] int quizId)
        {
            var quizExists = await _context.Quizzes.AnyAsync(q => q.Id == quizId);
            if(!quizExists)
            {
                return NotFound($"Квиз с Id = {quizId} не найден");
            }

            Question question = new Question { Text = dto.Text, Points = dto.Points, QuizId = quizId };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            QuestionDto questionDto = new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                Points = question.Points,
                QuizId = question.QuizId
            };

            return CreatedAtAction(nameof(GetById), new { quizId = question.QuizId, id = question.Id }, questionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateQuestionDto dto, [FromRoute] int quizId)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id && q.QuizId == quizId);
            if(question == null)
            {
                return NotFound($"Вопрос с Id = {id} не найден в квизе с quizId = {quizId}");
            }

            question.Text = dto.Text;
            question.Points = dto.Points;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromRoute] int quizId)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == id && q.QuizId == quizId);
            if (question == null)
            {
                return NotFound($"Вопрос с Id = {id} не найден в квизе с quizId = {quizId}");
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}