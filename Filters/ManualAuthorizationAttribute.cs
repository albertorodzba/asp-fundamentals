
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text;


using ToDoList.Models;


namespace ToDoList.Filters;

public class ManualAuthorizationAttribute : IActionFilter
{
    private readonly ILogger _logger;
    private readonly TodoContext _DBcontext;
    TodoToken token ;

    public ManualAuthorizationAttribute
    (
        ILogger< ManualAuthorizationAttribute> logger,
        TodoContext todoContext
    )
    {
        this._logger = logger;
        this._DBcontext = todoContext;
    }

    //BEFORE THE ACTION
    public async void OnActionExecuting(ActionExecutingContext context){
        
         this._logger.LogInformation("\nEntro al filtro");
        // string request;
        // using(StreamReader streamReader = new StreamReader(context.HttpContext.Request.Headers, Encoding.UTF8)){
        //     request = await streamReader.ReadToEndAsync();
        // }
        string bearerToken = context.HttpContext.Request.Headers.Authorization;
        if(bearerToken == null){
            _logger.LogInformation("\n No se envío ningun token");
            // context.Result = new JsonResult(new {error = "Error al iniciar sesión"});
            context.Result = new UnauthorizedResult();
        }
        else
        {
            
            bearerToken = bearerToken.Substring("Bearer ".Length);
            // _logger.LogError($"TOKEN REQUESTED {bearerToken}");
            // _logger.LogError($"TYPEOF {bearerToken.GetType()}");
            try{
                token = await this._DBcontext.TodoToken.SingleAsync(field => field.Token.Equals(bearerToken));
                if (token.expirationTime < DateTime.Now){
                    _logger.LogInformation("\n El token está caducado");
                    context.Result = new JsonResult(new {message = "token caducado, reinicie sesión"});
                }
            }catch(Exception ex){
                // context.Result = new UnauthorizedResult();
                _logger.LogError($"{ex.ToString()}");
                // Console.WriteLine($"EXCEPTION...{ex.ToString()}");

            }

            if(token == null){
                // context.Result = new JsonResult(new {error = "Error al iniciar sesión"});
                _logger.LogError("\n No se encontro el token en la base de datos");
                context.Result = new UnauthorizedResult();
            }
            else{
                _logger.LogError(token.Token);
                // context.Result = new JsonResult(new {BearerToken = bearerToken});
            }
            
        } 

        // context.Result = new JsonResult(new {message = "probando esta wea"});
        // context.Result = new JsonResult (new {Request = request});
    }
    //AFTER THE ACTION
    public void OnActionExecuted(ActionExecutedContext context){
        // this._logger.LogError("Saldrá del filtro");
    }

}