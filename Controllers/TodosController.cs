using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using System.Diagnostics;
using TodoAPI.Utility;
using CsvHelper;
using CsvHelper.Configuration;

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
    public ActionResult<List<Todo>> Get([FromQuery] TodoFilters? filters)
    {
      if (filters?.IsCompleted is null) return _context.Todos.ToList();

      return _context
          .Todos
          .Where(todo => todo.IsCompleted == filters.IsCompleted)
          .ToList();
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

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(long id, [FromBody] Todo? todo)
    {
      var todoFromDB = await _context.Todos.FindAsync(id);

      if (todoFromDB is null)
      {
        return NotFound();
      }

      todoFromDB.Title = todo.Title;
      todoFromDB.IsCompleted = todo.IsCompleted;

      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
      var todo = await _context.Todos.FindAsync(id);

      if (todo is null)
      {
        return NotFound();
      }

      _context.Todos.Remove(todo);

      await _context.SaveChangesAsync();

      return NoContent();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> Upload()
    {
      var file = Request.Form.Files[0];

      if (file.Length > 0)
      {
        var uploadedCSVPath = await FileUpload.Upload(file);

        if (uploadedCSVPath != null)
        {
          try
          {
            var config = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
              PrepareHeaderForMatch = args => args.Header.ToLower()
            };

            using var reader = new StreamReader(uploadedCSVPath);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Todo>();

            _context.AddRange(records.ToList());

            await _context.SaveChangesAsync();

            return Ok();
          } catch (Exception ex)
          {
            return BadRequest(ex.Message);
          }
        }

        return BadRequest();
      }

      return NoContent();
    }
  }
}
