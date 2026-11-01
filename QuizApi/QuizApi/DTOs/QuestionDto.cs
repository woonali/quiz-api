namespace QuizApi.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }
        public int QuizId { get; set; }

    }
}
