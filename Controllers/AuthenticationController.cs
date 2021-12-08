namespace portofolio_aspnet_core.Controllers;

public class AuthenticationController : AdminBaseController
{
    [Route(BaseUrl + "/login")]
    [AllowAnonymous]
    public IActionResult Index()
    {
        ViewData["Title"] = "Login";

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [Route("/login")]
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([Bind("Username, Password")] UserLogin userLogin)
    {
        var user = Authenticate(userLogin);

        if (user != null)
        {
            Generate(user);
            return RedirectToAction("Index", "Home");
        }

        TempData["error"] = "Username dan password tidak cocok";
        return RedirectToAction("Index", "Authentication");
    }

    [Route(BaseUrl + "/logout")]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToAction("Index", "Home");
    }

    private async void Generate(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme
        );

        var authProperties = new AuthenticationProperties 
        {
            AllowRefresh = true,
            ExpiresUtc = DateTime.Now.AddMinutes(15),
            IsPersistent = true,
            IssuedUtc = DateTime.Now
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );
    }

    private User? Authenticate(UserLogin userLogin)
    {
        var currentUser = Db?.Users?.FirstOrDefault(x => x.Username.ToLower() == userLogin.Username.ToLower() && x.Password == userLogin.Password);
        return currentUser;
    }
}