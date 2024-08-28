using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoList.Models;
using Npgsql;
namespace TodoList.Data;

public class AccountInfoContext : DbContext
{
    protected readonly IConfiguration _configuration;
    public DbSet<UserAccount> Accounts => Set<UserAccount>();
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    
    public AccountInfoContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration[DbConstants.DefaultConnectionStringPath]);
    }
}