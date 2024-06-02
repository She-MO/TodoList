using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;
using TodoList.ViewModels;

namespace TodoList.Controllers;

public class TodoItemController : Controller
{
    private readonly ILogger<AccountsController> _logger;
    readonly AccountInfoContext _db;
    public TodoItemController(ILogger<AccountsController> logger, AccountInfoContext accountInfoDb)
    {
        _logger = logger;
        _db = accountInfoDb;
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(TodoItemViewModel todoItem)
    {
        if (ModelState.IsValid)
        {
            int id = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
            TodoItem newItem = new TodoItem(todoItem);
            newItem.CreatedAt = DateTime.Now;
            newItem.User = await _db.Accounts.FirstOrDefaultAsync(user => user.Id == id);
            await _db.TodoItems.AddAsync(newItem);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        return View(todoItem);
    }
    
    public async Task<IActionResult> Edit(int? Id)
    {
        if (Id is null)
        {
            return NotFound();
        }

        TodoItem? item = await _db.TodoItems.FirstOrDefaultAsync(todoItem => todoItem.Id == Id);
        int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        if (item is null || item.UserAccountId != userId)
        {
            return NotFound();
        }
        TodoItemViewModel itemModel = new TodoItemViewModel(item);
        return View(itemModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TodoItemViewModel todoItemModel)
    {
        if (ModelState.IsValid)
        {
            //_logger.LogInformation("Model Is Valid");
            TodoItem? itemToUpdate = await _db.TodoItems.FirstOrDefaultAsync(item => item.Id == todoItemModel.Id);
            if (itemToUpdate is null)
            {
                return NotFound();
            }
            itemToUpdate.Name = todoItemModel.Name;
            itemToUpdate.Description = todoItemModel.Description;
            itemToUpdate.Deadline = todoItemModel.DeadlineDate.Add(todoItemModel.DeadlineTime.TimeOfDay);
            _db.TodoItems.Update(itemToUpdate); 
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        //_logger.LogInformation("Model Is invalid");
        return View(todoItemModel);
    }
    public async Task<IActionResult> Details(int? Id)
    {
        if (Id is null)
        {
            return NotFound();
        }

        TodoItem? item = await _db.TodoItems.FirstOrDefaultAsync(todoItem => todoItem.Id == Id);
        int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        if (item is null || item.UserAccountId != userId)
        {
            return NotFound();
        }
        return View(item);
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        TodoItem? itemToDelete = await _db.TodoItems.FirstOrDefaultAsync(item => item.Id == id);
        if (itemToDelete is null)
        {
            return NotFound();
        }

        _db.TodoItems.Remove(itemToDelete);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> Complete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        TodoItem? itemToComplete = await _db.TodoItems.FirstOrDefaultAsync(item => item.Id == id);
        if (itemToComplete is null)
        {
            return NotFound();
        }

        itemToComplete.IsDone = true;
        _db.TodoItems.Update(itemToComplete);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}