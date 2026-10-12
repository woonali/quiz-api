using System.ComponentModel.DataAnnotations;

namespace QuizApi.DTOs
{
    public class CreateThemeDto
    {
        [Required(ErrorMessage = "Название темы обязательно")]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; }
    }
}
