using Microsoft.AspNetCore.Mvc;

namespace portofolio_aspnet_core.Controllers;

public class HomeController : AdminBaseController
{
    [Route(BaseUrl + "/")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Home";
        
        return View();
    }
}
