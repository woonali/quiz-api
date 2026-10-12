using System.ComponentModel.DataAnnotations;

namespace QuizApi.DTOs
{
    public class CreateQuizDto
    {
        [Required(ErrorMessage = "Название викторины обязательно")]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }
        public int ThemeId { get; set; }
    }
}
