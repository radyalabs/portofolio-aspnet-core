using Microsoft.AspNetCore.Mvc;

namespace portofolio_aspnet_core.Controllers;

public class MemberController : AdminBaseController
{
    [Route(BaseUrl + "/members")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Members";
        
        var members = Db?.Members?.ToList();
        return View(members);
    }
}
