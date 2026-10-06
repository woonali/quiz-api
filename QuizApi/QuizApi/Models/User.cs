namespace QuizApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }

        public ICollection<Result> Results { get; set; }
    }
}
