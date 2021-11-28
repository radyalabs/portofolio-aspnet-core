using Microsoft.AspNetCore.Mvc;

namespace portofolio_aspnet_core.Controllers;

public class CategoryController : AdminBaseController
{
    [Route(BaseUrl + "/categories")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Categories";

        var categories = Db?.Categories?.ToList();
        return View(categories);
    }
}
