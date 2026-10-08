namespace QuizApi.DTOs
{
    public class SubmitQuizDto
    {
        public ICollection<int> SelectedAnswerOptionIds { get; set; }
    }
}
