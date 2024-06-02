using System.ComponentModel.DataAnnotations;

namespace TodoList.ViewModels;

public class UserNewAccauntModel
{
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Name { get; set; } = String.Empty;
    
    [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    [Required]
    public string Email { get; set; } = String.Empty;
    
    [StringLength(60, MinimumLength = 10)]
    [Required]
    public string Password { get; set; } = String.Empty;
}