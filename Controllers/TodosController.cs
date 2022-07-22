using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TodosController : ControllerBase
  {
    private readonly TodoContext _context;

    public TodosController (TodoContext context)
    {
      _context = context;
    }

    [HttpGet]
    public ActionResult<List<Todo>> Get()
    {
      return _context.Todos.ToList();
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> Post(Todo? todo)
    {
      if (todo is null)
      {
        return BadRequest();
      }

      if (todo.Id is null)
      {
        todo.Id = new Random().Next(0, int.MaxValue);
      }

      _context.Todos.Add(todo);

      await _context.SaveChangesAsync();

      return Created("", todo);
    }
  }
}
