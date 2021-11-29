namespace portofolio_aspnet_core.Models.Utilities;

public class BreadcrumbData 
{
    public string ActiveBreadcrumb { get; set; } = string.Empty;
    public List<Breadcrumb>? Breadcrumbs { get; set; }
}
public class Breadcrumb
{
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
