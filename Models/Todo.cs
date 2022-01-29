namespace TodoApi.Models
{
    public class Todo
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public bool IsDone { get; set; }
    }
}
