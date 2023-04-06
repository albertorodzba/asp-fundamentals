using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Diagnostics;
using ToDoList.Interfaces;


namespace ToDoList.Services;

public class JwtService: IJwtService{

    //settings
    private string secretKey = "Hola mundo soy Mario"; //20
    private int expireTime = 2;
    private readonly ILogger _logger;

    public JwtService(ILogger<JwtService> logger){
        this._logger = logger;
    }

    public string GenerateToken(int userId, string email){
        _logger.LogWarning("GENERATE TOKEN");
        
        //security key
        var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));

        //credentials
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //info on the payload
        List<Claim> claims = new List<Claim>{
            new Claim("id", userId.ToString()),
            new Claim("email", email)
        };
        
        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(2),
            signingCredentials: credentials
        );

        var tokenEncoded = new JwtSecurityTokenHandler().WriteToken(securityToken);
 
        
        return tokenEncoded;
    }

}