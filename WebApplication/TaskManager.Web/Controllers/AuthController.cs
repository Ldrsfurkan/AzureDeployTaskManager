using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.Web.Models.Auth;
using TaskManager.Web.Services;
using System.IdentityModel.Tokens.Jwt;

namespace TaskManager.Web.Controllers;

public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpGet("~/")]
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.Login(request);

        if (response != null && !string.IsNullOrEmpty(response.Token))
        {
            // 1. Read and parse the JWT Token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(response.Token);

            // 2. Extract the claims (ID, Email, Roles, etc.) from the token
            var claims = jwtToken.Claims.ToList();

            // 3. Create an identity and principal for MVC
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 4. Sign the user into the MVC application
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // 5. Store the raw token in a cookie so DutyService can use it for API requests
            Response.Cookies.Append("JwtToken", response.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            });

            // 6. Redirect to the GetDuties page!
            return RedirectToAction("MyTasks", "Duty");
        }

        ViewBag.ErrorMessage = "Login failed. Please check your credentials.";
        return View();
    }
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.Register(request);
        if (result != null)
        {
            // token saklanabilir (cookie vb.)
            return RedirectToAction("Login");
        }
        ViewBag.Error = "Kayıt başarısız";
        return View("Login");
    }
    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        // 1. Sign out of the MVC Cookie Authentication
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // 2. Delete the raw JWT token cookie we manually created
        Response.Cookies.Delete("JwtToken");

        // 3. Redirect the user back to the Login page
        return RedirectToAction("Login", "Auth");
    }
}