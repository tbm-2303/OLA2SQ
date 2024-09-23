using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OLA2SQ.Models;

namespace OLA2SQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTasksController : ControllerBase
    {
        private readonly TodoContext _context;

        public ToDoTasksController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoTask>>> GetTodoTasks()
        {
            return await _context.TodoTasks.ToListAsync();
        }

        // GET: api/ToDoTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoTask>> GetToDoTask(long id)
        {
            var toDoTask = await _context.TodoTasks.FindAsync(id);

            if (toDoTask == null)
            {
                return NotFound();
            }

            return toDoTask;
        }

        // PUT: api/ToDoTasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoTask(long id, ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDoTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDoTasks
        [HttpPost]
        public async Task<ActionResult<ToDoTask>> PostToDoTask(ToDoTask toDoTask)
        {
            _context.TodoTasks.Add(toDoTask);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetToDoTask", new { id = toDoTask.Id }, toDoTask);
            return CreatedAtAction(nameof(GetToDoTask), new { id = toDoTask.Id }, toDoTask);
        }

        // DELETE: api/ToDoTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoTask(long id)
        {
            var toDoTask = await _context.TodoTasks.FindAsync(id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            _context.TodoTasks.Remove(toDoTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoTaskExists(long id)
        {
            return _context.TodoTasks.Any(e => e.Id == id);
        }
    }
}
