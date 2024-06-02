using System.Diagnostics;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Controllers;
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    readonly AccountInfoContext _db;

    public HomeController(ILogger<HomeController> logger, AccountInfoContext accountInfoDb)
    {
        _db = accountInfoDb;
        _logger = logger;
    }
    public async Task<IActionResult> Index()
    {
        int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        return View(await _db.TodoItems.Where(item => item.UserAccountId == userId).ToListAsync());
    }
    public async Task<IActionResult> Filter(string searchString)
    {
        int userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id").Value);
        return View("~/Views/Home/Index.cshtml", await _db.TodoItems.Where(item => item.UserAccountId == userId && item.Name.Contains(searchString)).ToListAsync());
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}