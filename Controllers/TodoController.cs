using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core;
using Microsoft.EntityFrameworkCore;
using ToDoList.Filters;
using ToDoList.Models;


namespace ToDoList.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        //constructor
        public TodoController(TodoContext todoContext)
        {
            this._context = todoContext;
        }

        //return all the todos for the current user based on jwt
        [HttpGet]
         [ServiceFilter(typeof(ManualAuthorizationAttribute))]
        public async Task<ActionResult<List<TodoItem>>> GetTodoItems()
        {
            List<TodoItem> items;
            try
            {
                items = await this._context.TodoItems.ToListAsync();
                if (items == null)
                {
                    return new JsonResult(new { message = "No items found" });
                }
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
                throw;
            }


            // return Ok(items);
            // return items;
            return new JsonResult(new { message = "success, Items found", items });

        }

        [HttpGet("{id}")]//should be the same name to the argument
        public async Task<ActionResult<TodoItem>> GetItemById(int id)
        {
            TodoItem item = await this._context.TodoItems.FindAsync(id);
            if (item == null)
            {
                // return new JsonResult(new {message = "No item found"});
                return NotFound();
            }
            // return Ok(item);
            //return item;
            return new JsonResult(new { message = "success", item });

        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> AddItem(TodoItem item)
        {
            item.task_Created = DateTime.Now;
            this._context.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItems), new { id = item.ID }, item);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<TodoItem>> EditItem(/*[FromForm]*/ TodoItem todoItem, int id)
        {

            TodoItem itemToEdit = await this._context.TodoItems.FindAsync(id);

            itemToEdit.toDo = todoItem.toDo;
            itemToEdit.completed = todoItem.completed;

            await this._context.SaveChangesAsync();

            // return NoContent();
            return new JsonResult(new { success = true, message = "Item Updated" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<TodoItem>> DeleteItem(int id)
        {

            TodoItem itemToDelete = await this._context.TodoItems.FindAsync(id);
            this._context.TodoItems.Remove(itemToDelete);
            await this._context.SaveChangesAsync();
            return new JsonResult(new { successful = true, message = "Item deleted", itemToDelete });
        }


    }
}