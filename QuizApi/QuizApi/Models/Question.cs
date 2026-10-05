namespace QuizApi.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }
        public int QuizId { get; set; }
    }
}
