using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoUser>>> GetUsers()
        {
            IEnumerable<TodoUser> users = await this._context.TodoUsers.ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<TodoUser>> AddUser(TodoUser user)
        {

            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}