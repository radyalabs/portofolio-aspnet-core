namespace portofolio_aspnet_core.Controllers;

public class AdminBaseController : BaseController
{
    protected const string BaseUrl = "/admin";
    private FileController? _fileController;
    protected FileController? FileController => _fileController ??= HttpContext.RequestServices.GetService<FileController>();
}
