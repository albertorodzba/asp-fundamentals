using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class TodoContext: DbContext
    {
        public DbSet<TodoItem> TodoItems {get; set;}
        public DbSet<TodoUser> TodoUsers {get; set;}

        public TodoContext (DbContextOptions<TodoContext> options): base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<TodoItem>().Property(entity => entity.task_Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<TodoItem>().Property(entity => entity.completed).HasDefaultValue(false);

            // modelBuilder.Entity<TodoUser>().Property(entity => entity.Item);
            modelBuilder.Entity<TodoItem>()
            .HasOne<TodoUser>(t => t.User)
            .WithMany(u => u.Items)
            .HasForeignKey(t => t.UserId);
        }
    }
}