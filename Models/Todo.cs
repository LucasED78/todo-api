namespace TodoAPI.Models
{
  public class TodoFilters
  {
    public bool? IsCompleted { get; set; } = null;
  }

  public class Todo
  {
    public long? Id { get; set; }
    public string? Title { get; set; }
    public bool IsCompleted { get; set; }
  }
}
