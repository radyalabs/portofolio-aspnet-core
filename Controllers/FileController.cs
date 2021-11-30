namespace portofolio_aspnet_core.Controllers;

public class FileController
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<FileController> _logger;
    public FileController(IWebHostEnvironment webHostEnvironment, ILogger<FileController> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public string UploadFile(IFormFile? file)
    {
        string fileName = "";

        if (file != null)
        {
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "storage");
            fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadFolder, fileName);
            using var stream = File.Create(filePath);
            file.CopyTo(stream);
        }

        return fileName;
    }

    public bool DeleteFile(string path)
    {
        try
        {
            var fileName = path.Split("storage/").LastOrDefault();
            var filePath = $"{_webHostEnvironment.WebRootPath}/storage/{fileName}";
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("Catch", $"Catching error on FileController.DeleteFile: {e.Message}");
            return false;
        }
    }
}
