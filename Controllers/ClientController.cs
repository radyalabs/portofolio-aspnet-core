namespace portofolio_aspnet_core.Controllers;

public class ClientController : Controller
{
    private readonly ApplicationDbContext _db;
    public ClientController(ApplicationDbContext db)
    {
        _db = db;
    }

    [Route("/")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Radya Labs Technology - Portfolio App";

        ViewBag.Categories = _db.Categories?.Take(4).ToList();
        var projects = _db.Projects?
            .Include(x => x.ProjectImages)
            .Include(x => x.Categories)
            .ToList();
        return View(projects);
    }

    [Route("/{id}")]
    public IActionResult Detail(string id)
    {
        if (id == null) return RedirectToAction("Index", "Client");
        var project = _db.Projects?
            .Include(x => x.ProjectImages)
            .Include(x => x.Members)
            .Include(x => x.Categories)
            .FirstOrDefault(x => x.Id == id);
        if (project == null) return RedirectToAction("Index", "Client");

        ViewData["Title"] = project.Name + " - Radya Labs Technology";

        return View(project);
    }
}