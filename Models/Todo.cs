namespace TodoAPI.Models
{
  public class Todo
  {
    public long? Id { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
  }
}
