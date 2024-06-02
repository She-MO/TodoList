using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoList.Models;

public class UserAccount
{
    public int Id { get; set; }
    
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Name { get; set; } = String.Empty;
    
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    [Required]
    public string Email { get; set; } = String.Empty;
    
    [StringLength(60, MinimumLength = 10)]
    [Required]
    public string Password { get; set; } = String.Empty;
    public List<TodoItem> TodoItems { get; set; } = new();

    public UserAccount() {}
}