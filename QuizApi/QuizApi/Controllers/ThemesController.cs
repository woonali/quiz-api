using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Data;
using QuizApi.DTOs;
using QuizApi.Models;

namespace QuizApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThemesController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public ThemesController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theme>>> GetAll()
        {
            var themes = await _context.Themes.ToListAsync();

            return Ok(themes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Theme>> GetById(int id)
        {
            var theme = await _context.Themes.FindAsync(id);

            if (theme == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(theme);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Theme>> Create(CreateThemeDto dto)
        {
            Theme theme = new Theme { Title = dto.Title };
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = theme.Id }, theme);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateThemeDto dto)
        {
            var theme = await _context.Themes.FindAsync(id);
            if(theme == null)
            {
                return NotFound();
            }

            theme.Title = dto.Title;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            _context.Themes.Remove(theme);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
