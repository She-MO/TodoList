using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

public class AccountInfoContext : DbContext
{
    public DbSet<UserAccount> Accounts => Set<UserAccount>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    
    public AccountInfoContext() => Database.EnsureCreated();
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\Users\\User\\RiderProjects\\TodoList\\TodoList\\Data\\users.db");
    }
}