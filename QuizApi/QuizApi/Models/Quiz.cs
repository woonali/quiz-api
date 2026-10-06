namespace QuizApi.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ThemeId { get; set; }

        public Theme Theme { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Result> Results { get; set; }
    }
}
