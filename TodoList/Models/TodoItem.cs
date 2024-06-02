using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoList.ViewModels;

namespace TodoList.Models;

public class TodoItem
{
    public TodoItem() {}
    public TodoItem(TodoItemViewModel todoItem)
    {
        this.Name = todoItem.Name;
        this.Description = todoItem.Description;
        this.Deadline = todoItem.DeadlineDate.Add(todoItem.DeadlineTime.TimeOfDay);
        this.Id = todoItem.Id;
    }
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    [Display(Name = "Created at")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:g" +
        "}")]
    public DateTime Deadline { get; set; }
    [Display(Name = "Is completed")]
    public bool IsDone { get; set; } = false;
    public int UserAccountId { get; set; }
    public UserAccount? User { get; set; }
    
    public void Edit(TodoItemViewModel todoItem)
    {
        this.Name = todoItem.Name;
        this.Description = todoItem.Description;
        this.Deadline = todoItem.DeadlineDate.Add(todoItem.DeadlineTime.TimeOfDay);
    }
}