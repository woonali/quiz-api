using Microsoft.EntityFrameworkCore;
using QuizApi.Models;

namespace QuizApi.Data
{
    public class QuizDbContext : DbContext
    {
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }
    }
}
