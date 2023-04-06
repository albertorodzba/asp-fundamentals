
namespace ToDoList.Interfaces;

public interface IJwtService{
    
    public string GenerateToken(int userId, string email);
    // public bool CheckToken(string token);
}