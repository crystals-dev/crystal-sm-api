using Amethyst;
using CrystalApi.Data;
using CrystalApi.Library;
using CrystalApi.Models;
using CrystalApi.Services;
using CrystalApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace CrystalApi.Controllers;

[ApiController]
[Route("v1/users")]
public class UserController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserViewModel model, [FromServices] DbDataContext context, [FromServices] EmailService emailService)
    {
        var existUser = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (existUser != null)
        {
            return BadRequest(new
            {
                success = false,
                message = "This email is already in use!"
            });
        }

        var hash = PasswordHasher.Hash(model.Password);
        
        var user = new User
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Password = hash
        };
        var auth = new Auth()
        {
            Code = NumberGenerator.RandomNum(),
            UserId = user.Id
        };
        await context.Users.AddAsync(user);
        await context.Auths.AddAsync(auth);
        await context.SaveChangesAsync();
        
        string path = "/Users/daniellopes/projects/dotnet/CrystalApi/Views/confirm_email.html";
        string contents = await System.IO.File.ReadAllTextAsync(path);
        var htmlCompiler = new MailCompiler<EmailConfirmViewModel>(contents);
        var templateVars = new EmailConfirmViewModel()
        {
            Name = model.FirstName,
            Code = auth.Code
        };
        var body = htmlCompiler.Compile(templateVars);
        Console.WriteLine(body);
        var sendMail = emailService.Send(model.FirstName, model.Email, "Confirm your email", body);
        if (!sendMail)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Can't send the verification email"
            });
        }

        return Ok(new
        {
            success = true
        });
    }

    [HttpPost("re-send")]
    public async Task<IActionResult> Resend([FromBody] ResendViewModel model, [FromServices] DbDataContext context, [FromServices] EmailService emailService)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found!"
            });
        }

        var auth = await context.Auths.FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (auth == null)
        {
            auth = new Auth()
            {
                UserId = user.Id,
                Code = NumberGenerator.RandomNum()
            };
            await context.Auths.AddAsync(auth);
            await context.SaveChangesAsync();
        }
        string path = "/Users/daniellopes/projects/dotnet/CrystalApi/Views/confirm_email.html";
        string contents = await System.IO.File.ReadAllTextAsync(path);
        var htmlCompiler = new MailCompiler<EmailConfirmViewModel>(contents);
        var templateVars = new EmailConfirmViewModel()
        {
            Name = user.FirstName,
            Code = auth.Code
        };
        var body = htmlCompiler.Compile(templateVars);
        Console.WriteLine(body);
        var sendMail = emailService.Send(user.FirstName, model.Email, "Confirm your email", body);
        if (!sendMail)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Can't send the verification email"
            });
        }

        return Ok(new
        {
            success = true
        });
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyViewModel model, [FromServices] DbDataContext context, [FromServices] TokenService tokenService)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found!"
            });
        }

        var auth = await context.Auths.FirstOrDefaultAsync(x => x.UserId == user.Id);
        if (auth == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Confirm request failed!"
            });
        }

        if (auth.Code != model.Code)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid code!"
            });
        }

        user.Confirmed = true;
        context.Users.Update(user);
        context.Auths.Remove(auth);
        await context.SaveChangesAsync();
        return Ok(new
        {
            success = true,
            token = tokenService.GenerateToken(user)
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] DbDataContext context,
        [FromServices] TokenService tokenService)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
        {
            return BadRequest(new
            {
                success = false,
                message = "Wrong email or password"
            });
        }

        if (!PasswordHasher.Verify(user.Password, model.Password))
        {
            return BadRequest(new
            {
                success = false,
                message = "Wrong email or password"
            });
        }

        return Ok(new
        {
            success = true,
            token = tokenService.GenerateToken(user)
        });
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get([FromServices] DbDataContext context)
    {
        var user = await context.Users
            .Select(x => new { x.Email, x.FirstName, x.LastName, x.Confirmed, x.Verified, x.ProfilePhoto, x.CreatedAt })
            .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);
        if (user == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User not found!"
            });
        }

        return Ok(new
        {
            success = true,
            user
        });
    }
}