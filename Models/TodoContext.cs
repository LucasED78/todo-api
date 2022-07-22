﻿using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;

namespace TodoAPI.Models
{
  public class TodoContext : DbContext
  {
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
    public DbSet<TodoAPI.Models.Todo>? Todo { get; set; }
  }
}
