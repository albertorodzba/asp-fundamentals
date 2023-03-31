using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Models
{
    public class TodoItem
    {
        [Key]
        public int ID {get; set;}
        [Required]
        public string toDo {get; set;}

        public DateTime task_Created {get; set;}

        [Required]
        public Boolean completed{get; set;}

        public int UserId {get; set;}
        public TodoUser? User{get; set;}

    }
}