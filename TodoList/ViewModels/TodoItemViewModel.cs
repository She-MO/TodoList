using System.ComponentModel.DataAnnotations;
using TodoList.Models;

namespace TodoList.ViewModels;

public class TodoItemViewModel
{
    public TodoItemViewModel() {}
    public TodoItemViewModel(TodoItem item)
    {
        this.Id = item.Id;
        this.Name = item.Name;
        this.Description = item.Description;
        this.DeadlineTime = DateTime.MinValue.Add(item.Deadline.TimeOfDay);
        this.DeadlineDate = item.Deadline.Date;
    }
    public int? Id { get; set; }
    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string? Name { get; set; }
    [StringLength(150)]
    public string? Description { get; set; }
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    [Required]
    public DateTime DeadlineDate { get; set; }
    [Display(Name = "Time")]
    [DataType(DataType.Time)]
    [Required]
    public DateTime DeadlineTime { get; set; }
}