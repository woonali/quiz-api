using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Data;
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
    }
}
