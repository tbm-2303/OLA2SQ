using Microsoft.EntityFrameworkCore;

namespace OLA2SQ.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoTask> TodoTasks { get; set; } = null!;
    }
}
