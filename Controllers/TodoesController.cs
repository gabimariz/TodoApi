using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Models.Input;

namespace TodoApi.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class TodoesController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodoesController(TodoDbContext context)
        {
            _context = context;
        }

        /**
         * <summary>
         *      This property returns all tasks in the database
         * </summary>
         * <returns>Returns a to-do list</returns>
         * <response code="200">Return to task list</response>
         * <response code="204">If there is no task</response>
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAsync()
        {
            var todo = await _context.Todo!.ToListAsync();
            
            if(todo.Count == 0)
            {
                return NoContent();
            }

            return todo;
        }

        /**
         * <summary>
         *      Update a task
         * </summary>
         * <returns>Get the task id and edit the data</returns>
         * <param name="id">Task id</param>
         * <param name="input">Task entry to be updated</param>
         * <response code="200">Updated task</response>
         * <response code="400">Conflict between identities</response>
         * <response code="404">Task not found</response>
         */
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute]Guid id, [FromBody]EditTodoInputModel input)
        {
            var todo = new Todo
            {
                Id = input.Id,
                Title = input.Title,
                IsDone = false,
            };

            if(id != input.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(input);
        }

        /**
        * <summary>
        *      Create a task
        * </summary>
        * <returns>Create a new task and add to the database</returns>
        * <param name="input">Get the data to be created</param>
        * <response code="200">Created task</response>
        * <response code="400">Conflict between identities</response>
        */
        [HttpPost]
        public async Task<ActionResult<Todo>> PostAsync([FromBody] CreateTodoInputModel input)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var todo = new Todo
            {
                Title = input.Title,
                IsDone = false,
            };

            _context.Todo!.Add(todo);

            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        /**
        * <summary>
        *      Delete a task
        * </summary>
        * <returns>delete a task from the database</returns>
        * <param name="id">Get the task id</param>
        * <response code="200">Task deleted</response>
        * <response code="404">Task not found</response>
        */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            var todo = await _context.Todo!.FindAsync(id);

            if(todo == null)
            {
                return NotFound();
            }

            _context.Todo.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok("Task deleted!");
        }

        private bool TodoExists(Guid id)
        {
            return _context.Todo!.Any(e => e.Id == id);
        }
    }
}
