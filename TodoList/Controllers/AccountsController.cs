using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using TodoList.Data;
using TodoList.Models;
using TodoList.ViewModels;

namespace TodoList.Controllers;

public class AccountsController : Controller
{
    private readonly ILogger<AccountsController> _logger;
    readonly AccountInfoContext _db;
    private readonly IPasswordHasher<UserAccount> _passwordHasher;

    private bool PasswordIsCorrect(UserAccount user, string hashedPass, string providedPass)
    {
        return _passwordHasher.VerifyHashedPassword(user, hashedPass, providedPass) ==
               PasswordVerificationResult.Success;
    }

    public AccountsController(ILogger<AccountsController> logger,AccountInfoContext accountInfoDb, IPasswordHasher<UserAccount> passwordHasher)
    {
        _logger = logger;
        _db = accountInfoDb;
        _passwordHasher = passwordHasher;
    }
    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult CreateAccount()
    {
        return View("~/Views/Accounts/NewAccount.cshtml");
    }
    [HttpPost]
    public async Task<IActionResult> CreateAccount([Bind("Name,Email,Password")] UserNewAccauntModel user, string? returnUrl)
    {
        if (ModelState.IsValid && await _db.Accounts.FirstOrDefaultAsync(account => account.Email == user.Email) == null)
        {
            UserAccount newUser = new UserAccount() {Name = user.Name, Email = user.Email};
            newUser.Password = _passwordHasher.HashPassword(newUser, user.Password);
            await _db.AddAsync(newUser);
            await _db.SaveChangesAsync();
            newUser = await _db.Accounts.FirstOrDefaultAsync(u => u.Email == user.Email);
            List<Claim> claims = new List<Claim> { 
                new Claim(ClaimTypes.Email, newUser.Email), 
                new Claim(ClaimTypes.Name, newUser.Name), 
                new Claim("Id", newUser.Id.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(10),
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Home");
        }
        return View("~/Views/Accounts/NewAccount.cshtml", user);
    }
    [HttpPost]
    public async Task<IActionResult> SignIn([Bind("Email,Password")] UserSignInModel user, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
             UserAccount? checkUser = await _db.Accounts.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (checkUser is null || !PasswordIsCorrect(checkUser, checkUser.Password, user.Password))
            {
                return View("~/Views/Accounts/SignIn.cshtml", user);
            }
            List<Claim> claims = new List<Claim> { 
                new Claim(ClaimTypes.Email, checkUser.Email), 
                new Claim(ClaimTypes.Name, checkUser.Name), 
                new Claim("Id", checkUser.Id.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true,
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return RedirectToAction("Index", "Home");
        }
        return View("~/Views/Accounts/SignIn.cshtml", user);
    }
}