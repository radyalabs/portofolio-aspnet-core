namespace portofolio_aspnet_core.Controllers;

public class BaseController : Controller
{
    private ILogger<BaseController>? _logger;
    private ApplicationDbContext? _db;

    protected ILogger<BaseController>? Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<BaseController>>();
    protected ApplicationDbContext? Db => _db ??= HttpContext.RequestServices.GetService<ApplicationDbContext>();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
