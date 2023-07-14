
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models;

public class TodoToken{

    [Key]
    public int Id {get; set;}

    public string Token {get; set;}

    public DateTime expirationTime {get; set;}

    public int todoUserId {get; set;}
    public TodoUser todoUser {get; set;}
}