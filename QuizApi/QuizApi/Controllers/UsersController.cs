using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Data;
using QuizApi.DTOs;

namespace QuizApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly QuizDbContext _context;
        public UsersController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _context.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Nickname = u.Nickname,
                Email = u.Email
            }).ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _context.Users.Where(u => u.Id == id).Select(u => new UserDto
            {
                Id = u.Id,
                Nickname = u.Nickname,
                Email = u.Email
            }).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
