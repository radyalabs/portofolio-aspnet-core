using Microsoft.AspNetCore.Mvc;

namespace portofolio_aspnet_core.Controllers;

public class ProjectController : AdminBaseController
{
    [Route(BaseUrl + "/projects")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Projects";
        
        var projects = Db?.Projects?.ToList();
        return View(projects);
    }
}
