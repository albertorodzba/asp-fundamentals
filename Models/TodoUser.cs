using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ToDoList.Models
{
    public class TodoUser
    {
        [Key]
        public int UserID {get; set;}

        [Required]
        public string Full_Name {get; set;}

        [Required]
        public string  Email {get; set;}

        [Required]
        public string Password {get; set;}

        [JsonIgnore]
        public List<TodoItem>? Items{get; set;}

        
    }
}