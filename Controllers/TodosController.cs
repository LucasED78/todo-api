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
  }
}
