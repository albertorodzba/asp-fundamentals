using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using ToDoList.DTO;
using ToDoList.Models;
using ToDoList.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ToDoList.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IPasswordHasher<TodoUser> _passwordHasher;
        private readonly IJwtService _jwt;

        public UserController(TodoContext context, IPasswordHasher<TodoUser> passwordHasher, IJwtService jwtService )
        {
            this._context = context;
            this._passwordHasher = passwordHasher;
            this._jwt = jwtService;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoUser>>> GetUsers()
        {
            IEnumerable<TodoUser> users = await this._context.TodoUsers.ToListAsync();

            return Ok(users);
        }

        
        [HttpPost]
        public async Task<ActionResult<TodoUser>> AddUser(TodoUser user)
        {

            string hashedPassword = _passwordHasher.HashPassword(user, user.Password);
            user.Password = hashedPassword;
            Debug.WriteLine(hashedPassword);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<login>> Login(login loginDto) //<-- make a dto for receive those parameters
        {
            
            TodoUser user =  await _context.TodoUsers.SingleAsync(e => e.Email.Equals(loginDto.Email));
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.password);
            Debug.WriteLine("success", user.Full_Name, user.Password);
            if (result == PasswordVerificationResult.Success) Debug.WriteLine("success", user.Full_Name, user.Password);

            //jwt
            string jwt = _jwt.GenerateToken( user.UserID,  user.Email);
            return Ok(jwt);
            // return Ok(user);
        }
    }
}