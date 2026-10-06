namespace QuizApi.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int TotalScore { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }

        public User User { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
