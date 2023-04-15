using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using ToDoList.DTO;
using ToDoList.Models;
using ToDoList.Interfaces;
using ToDoList.Filters;

namespace ToDoList.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IPasswordHasher<TodoUser> _passwordHasher;
        private readonly IJwtService _jwt;
        private readonly ILogger _logger;

        public UserController(TodoContext context, IPasswordHasher<TodoUser> passwordHasher, IJwtService jwtService, ILogger<UserController> logger)
        {
            this._context = context;
            this._passwordHasher = passwordHasher;
            this._jwt = jwtService;
            this._logger = logger;
        }
        // [Authorize]
        [ServiceFilter(typeof(ManualAuthorizationAttribute))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoUser>>> GetUsers()
        {

            IEnumerable<TodoUser> users = await this._context.TodoUsers.ToListAsync();
            // _logger.LogError("Entro aun asi al metodo");
            return Ok(users);
        }

        
        [HttpPost]
        public async Task<ActionResult<TodoUser>> AddUser(TodoUser user)
        {
            this._logger.LogInformation($"\n  OBJETO ENTRANTE: {user.Email}, {user.Full_Name}, {user.Password}");
            string hashedPassword = _passwordHasher.HashPassword(user, user.Password);
            user.Password = hashedPassword;
            Debug.WriteLine(hashedPassword);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // [ServiceFilter(typeof(ManualAuthorizationAttribute))]
        [HttpPost("login")]
        public async Task<ActionResult<login>> Login(login loginDto) //<-- make a dto for receive those parameters
        {
            
            TodoUser user =  await _context.TodoUsers.SingleAsync(e => e.Email.Equals(loginDto.Email));
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.password);
            Debug.WriteLine("success", user.Full_Name, user.Password);

            if (result != PasswordVerificationResult.Success) return new UnauthorizedResult();

            //if the user has an row with a token expired, update the row with a new token
            TodoToken token ;
            string jwt;
            try{

                token = await _context.TodoToken.FirstAsync(field => field.todoUserId == user.UserID);

            }catch(Exception ex){
                _logger.LogInformation("Creando token por primera vez");
                jwt = _jwt.GenerateToken( user.UserID,  user.Email);
                
                //save on the database to authorize manually
                TodoToken newToken = new TodoToken();
                newToken.Token = jwt;
                newToken.expirationTime = DateTime.Now.AddMinutes(2);
                newToken.todoUserId = user.UserID;
                await this._context.AddAsync(newToken);
                await this._context.SaveChangesAsync(); 
                return Ok(jwt);
            }
            
            if (token.expirationTime < DateTime.Now){
                _logger.LogInformation("El token expiro, creando uno nuevo...");
                jwt = _jwt.GenerateToken( user.UserID,  user.Email);
                token.expirationTime = DateTime.Now.AddMinutes(2);
                token.Token = jwt;
                this._context.Update(token);
                await this._context.SaveChangesAsync();
                return Ok(jwt);
            }

            
            _logger.LogInformation("El token aun es válido, retornando el token almacenado");
             return Ok(token.Token);
        }
    }
}