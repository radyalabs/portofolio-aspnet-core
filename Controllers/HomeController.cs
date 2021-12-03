namespace portofolio_aspnet_core.Controllers;

public class HomeController : AdminBaseController
{
    [Route(BaseUrl + "/")]
    [Authorize]
    public IActionResult Index()
    {
        ViewData["Title"] = "Home";
        
        return View();
    }
}
