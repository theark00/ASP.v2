using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModels;
using Microsoft.Extensions.Logging; 

namespace WebApp.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly ApplicationContext _context;
    private readonly ILogger<AuthController> _logger; // Declare ILogger<AuthController>

    public AuthController(
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager,
        ApplicationContext context,
        ILogger<AuthController> logger) // Inject ILogger<AuthController>
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger; // Assign logger instance
    }

    //signup
    [Route("/signup")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [Route("/signup")]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (!await _context.Users.AnyAsync(x => x.Email == model.Email))
            {
                var userEntity = new UserEntity
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                try
                {
                    var result = await _userManager.CreateAsync(userEntity, model.Password);
                    if (result.Succeeded)
                    {
                        if ((await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false)).Succeeded)
                            return LocalRedirect("/");

                        return LocalRedirect("/signin");
                    }
                    else
                    {
                        // Handle specific error cases
                        foreach (var error in result.Errors)
                        {
                            if (error.Code == "DuplicateUserName")
                            {
                                ViewData["StatusMessage"] = "A user with the same email already exists.";
                                return BadRequest(); // Return 400 Bad Request
                            }
                            else if (error.Code == "PasswordTooShort")
                            {
                                ViewData["StatusMessage"] = "The password must be at least 8 characters long.";
                                return BadRequest(); // Return 400 Bad Request
                            }
                            // Handle other error cases if needed
                        }

                        // If no specific error is caught, return a generic error message
                        ViewData["StatusMessage"] = "Something went wrong. Please try again later or contact customer service.";
                        return StatusCode(500); // Return 500 Internal Server Error
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred during user registration.");

                    // Return a generic error message
                    ViewData["StatusMessage"] = "Something went wrong. Please try again later or contact customer service.";
                    return StatusCode(500); // Return 500 Internal Server Error
                }
            }
            else
            {
                ViewData["StatusMessage"] = "A user with the same email already exists.";
                return Conflict(); // Return 409 Conflict
            }
        }

        // Return validation errors if ModelState is not valid
        return BadRequest(ModelState); // Return 400 Bad Request
    }



    [Route("/signin")]

//login
public IActionResult SignIn(string returnUrl)
{

    ViewData["ReturnUrl"] = returnUrl ?? "/";
    return View();
}
[HttpPost]
[Route("/signin")]
public async Task<IActionResult> SignIn(SignInViewModel model, string returnUrl)
{
    if (ModelState.IsValid)
    {
        if ((await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.IsPresistent, false)).Succeeded)
            return LocalRedirect(returnUrl);
    }
    ViewData["ReturnUrl"] = returnUrl;
    ViewData["StatusMessage"] = "Incorrect email or password";
    return View(model);
}

//logout
[Route("/signout")]
public new async Task<IActionResult> SignOut()
{
    await _signInManager.SignOutAsync();
    return RedirectToAction("Home", "Default");
}
}