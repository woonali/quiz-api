using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApi.Data;
using QuizApi.DTOs;
using QuizApi.Models;

namespace QuizApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly QuizDbContext _context;
        
        public AuthController(QuizDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterUserDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Email == dto.Email || u.Nickname == dto.Nickname);
            if (userExists)
            {
                return BadRequest("Пользователь с таким email или никнеймом уже существует");
            }

            var user = new User
            {
                Nickname = dto.Nickname,
                Email = dto.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Email = user.Email
            };
            
            return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = user.Id }, userDto);
        }
    }
}
