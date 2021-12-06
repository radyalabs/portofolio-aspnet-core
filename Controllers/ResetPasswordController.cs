using System.Web;

namespace portofolio_aspnet_core.Controllers;

[AllowAnonymous]
public class ResetPasswordController : AdminBaseController
{
    private readonly IConfiguration _config;
    public ResetPasswordController(IConfiguration config)
    {   
        _config = config;
    }

    [Route(BaseUrl + "/reset-password")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Reset Password";

        return View();
    }

    [Route(BaseUrl + "/reset-password")]
    [HttpPost]
    public IActionResult Reset([Bind("Email")] string email)
    {
        if (
            Db?.Users == null || 
            Db?.ResetPasswords == null ||
            !Db.Users.Any(x => x.Email == email))
        {
            TempData["error"] = "Email is not registered";
            return RedirectToAction("Index", "ResetPassword");
        }

        var user = Db.Users.FirstOrDefault(x => x.Email == email);
        if (user == null)
        {
            TempData["error"] = "Email is not registered";
            return RedirectToAction("Index", "ResetPassword");
        }

        var resetPasswordId = Guid.NewGuid().ToString();
        var token = GenerateToken(resetPasswordId);

        Db?.ResetPasswords.Add(new ResetPassword
        {
            Id = resetPasswordId,
            UserId = user.Id,
            Token = token,
            IssuedAt = DateTime.Now.ToUniversalTime()
        });
        Db?.SaveChanges();

        var changePasswordUrl = Request.Scheme + "://" + Request.Host + Url.Action("ResetPasswordView", "ResetPassword", new { token = token } );

        MimeMessage message = new MimeMessage();
        message.From.Add(new MailboxAddress("Portfolio App", "portfolio.app.2021@gmail.com"));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = "Reset Password Link";
        message.Body = new TextPart("html")
        {
            Text = $@"
            <div style='color: #000000; font-family: Helvetica; font-size: 12px;'>
            <h1>Hello {user.Username},</h1>
            <p>
              <strong>A request has been received to reset password of your account.</strong>
            </p>
            <div style='display: flex;justify-content: center;'>
              <a
                href='{changePasswordUrl}'
                style='
                margin-left: 10px;
                background-color: #0d6efd;
                border: none;
                color: #fff;
                box-shadow: 1px 1px 5px 1px gray;
                text-decoration: none;
                padding: 12px;
                '
            >
                Reset Password
            </a>
            </div>
            <p style='color: gray'>Ignore this email if your did not initiate this request.</p>
            <p style='color: gray'>Thank you,<br/>
            Portfolio App</p>
            </div>
            "
        };

        SmtpClient client = new SmtpClient();
        try
        {
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(_config["EmailConfig:Email"], _config["EmailConfig:Password"]);
            client.Send(message);
        }
        catch (Exception ex)
        {
            Logger?.LogError("Catching error on ResetPassword.Reset: " + ex.Message.ToString());
            TempData["error"] = "Something went wrong, please try again later";
            return RedirectToAction("Index", "ResetPassword");
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }

        TempData["success"] = "Reset password link has been sent to your email";
        return RedirectToAction("Index", "Authentication");
    }

    
    [Route(BaseUrl + "/reset-password/{token}")]
    public IActionResult ResetPasswordView(string token)
    {
        ViewData["Title"] = "Change Password";

        if (
            token == null || 
            Db?.ResetPasswords == null || 
            !Db.ResetPasswords.Any(x => x.Token == token)
        )
        {
            TempData["error"] = "Password reset link is not valid";
            return RedirectToAction("Index", "Authentication");
        }

        var resetToken = Db.ResetPasswords.FirstOrDefault(x => x.Token == token);
        if (resetToken == null)
        {
            TempData["error"] = "Password reset link is not valid";
            return RedirectToAction("Index", "Authentication");
        }

        return View();
    }

    [Route(BaseUrl + "/reset-password/{token}")]
    [HttpPost]
    public IActionResult ResetPassword(string token, [Bind("Password, PasswordConfirmation")] ChangePassword changePassword)
    {
        if(changePassword.Password != changePassword.PasswordConfirmation)
        {
            TempData["error"] = "Password and confirmation password did not matched";
            return RedirectToAction("ResetPasswordView", "ResetPassword", new { token = token });
        }
        
        if (
            token == null || 
            Db?.ResetPasswords == null || 
            Db?.Users == null || 
            !Db.ResetPasswords.Any(x => x.Token == token)
        )
        {
            TempData["error"] = "Password reset link is not valid - 1";
            return RedirectToAction("Index", "Authentication");
        }

        var resetToken = Db.ResetPasswords.FirstOrDefault(x => x.Token == token);
        var identity = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        if (resetToken == null || identity == null)
        {
            TempData["error"] = "Password reset link is not valid - 2";
            return RedirectToAction("Index", "Authentication");
        }

        var resetPassword = Db.ResetPasswords.Find(identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value);
        if (resetPassword == null)
        {
            TempData["error"] = "Password reset link is not valid - 3";
            return RedirectToAction("Index", "Authentication");
        }

        var user = Db?.Users.FirstOrDefault(x => x.Id == resetPassword.UserId);
        if (user == null)
        {
            TempData["error"] = "Password reset link is not valid - 4";
            return RedirectToAction("Index", "Authentication");
        }

        user.Password = changePassword.Password;
        Db?.Users.Update(user);
        Db?.ResetPasswords.Remove(resetPassword);
        Db?.SaveChanges();

        TempData["success"] = "Password successfuly changed";
        return RedirectToAction("Index", "Authentication");
    }

    [Route(BaseUrl + "/change-password")]
    [Authorize]
    public IActionResult ChangePasswordView()
    {
        var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = Db?.Users?.Find(id);
        if (user == null) return RedirectToAction("Index", "Home");
        
        return View();
    }

    [Route(BaseUrl + "/change-password")]
    [Authorize]
    [HttpPost]
    public IActionResult ChangePassword([Bind("Password", "PasswordConfirmation")] ChangePassword changePassword)
    {
        if (changePassword.Password != changePassword.PasswordConfirmation)
        {
            TempData["error"] = "Password and confirmation password did not matched";
            return RedirectToAction("ChangePasswordView", "ResetPassword");
        }

        var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = Db?.Users?.Find(id);
        if (user == null) return RedirectToAction("Index", "Home");
        
        user.Password = changePassword.Password;
        Db?.Users?.Update(user);
        Db?.SaveChanges();

        TempData["success"] = "Password successfully changed";
        return RedirectToAction("Index", "Home");
    }

    private string GenerateToken(string resetPasswordId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, resetPasswordId)
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"], 
            _config["Jwt:Audience"], 
            claims, 
            expires: DateTime.Now.AddMinutes(15), 
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}