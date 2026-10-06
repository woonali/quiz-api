namespace QuizApi.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int ResultId { get; set; }
        public int AnswerOptionId { get; set; }

        public Result Result { get; set; }
        public AnswerOption AnswerOption { get; set; }
    }
}
