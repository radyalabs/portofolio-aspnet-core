using Microsoft.AspNetCore.Mvc;
using portofolio_aspnet_core.Models;
using portofolio_aspnet_core.Models.Utilities;
using Microsoft.EntityFrameworkCore;

namespace portofolio_aspnet_core.Controllers;

public class ProjectController : AdminBaseController
{
    [Route(BaseUrl + "/projects")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Projects";
        ViewData["ActiveBreadcrumb"] = "Project";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>();
        
        var projects = Db?.Projects?.Include(x => x.Members).Include(x => x.Categories).ToList();
        return View(projects);
    }

    [Route(BaseUrl + "/projects/create")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Project";
        ViewData["ActiveBreadcrumb"] = "Create Project";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Title = "Project",
                Url = Url.Action("Index", "Project") ?? "#"
            }
        };

        ViewBag.Categories = Db?.Categories?.ToList();
        ViewBag.Members = Db?.Members?.ToList();
        return View();
    }

    [Route(BaseUrl + "/projects/create")]
    [HttpPost]
    public IActionResult Store([Bind("Name, Description, Url, Content, Images, MemberIds, CategoryIds")] ProjectInput projectInput)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Project failed to create";
            return Json("Input data is not valid");
        }

        using var transaction = Db?.Database.BeginTransaction();
        try
        {
            var projectId = Guid.NewGuid().ToString();
            Db?.Add(new Project
            {
                Id = projectId,
                Name = projectInput.Name,
                Description = projectInput.Description,
                Url = projectInput.Url,
                Content = projectInput.Content
            });
            if (projectInput.MemberIds != null && projectInput.MemberIds.Count > 0)
            {
                var memberIds = projectInput.MemberIds.FirstOrDefault()?.Split(",");
                var members = memberIds?.Select(x => new ProjectMember
                {
                    Id = Guid.NewGuid().ToString(),
                    MemberId = x,
                    ProjectId = projectId
                }).ToList();
                if (members != null)
                {
                    Db?.AddRange(members);
                }
            }
            if (projectInput.CategoryIds != null && projectInput.CategoryIds.Count > 0)
            {
                var categoryIds = projectInput.CategoryIds.FirstOrDefault()?.Split(",");
                var categories = categoryIds?.Select(x => new ProjectCategory
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryId = x,
                    ProjectId = projectId
                }).ToList();
                if (categories != null)
                {
                    Db?.ProjectCategories?.AddRange(categories);
                }
            }
            if (projectInput.Images != null && projectInput.Images.Count > 0)
            {
                var images = projectInput.Images.Select(x => new ProjectImage
                {
                    Id = Guid.NewGuid().ToString(),
                    ProjectId = projectId,
                    Url = $"{Request.Scheme}://{Request.Host}/storage/{FileController?.UploadFile(x)}"
                }).ToList();
                Db?.AddRange(images);
            }
            
            Db?.SaveChanges();
            transaction?.Commit();
            TempData["success"] = "Project successfully created!";
            return Content(Url.Action("Index", "Project") ?? "");
        }
        catch (Exception e)
        {
            string message = $"Catching error on Create Project: {e.Message}";
            TempData["error"] = "Project failed to create";
            return Json(e);
        }
    }

    [Route(BaseUrl + "/projects/{id}")]
    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewData["Title"] = "Edit Project";
        ViewData["ActiveBreadcrumb"] = "Edit Project";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Title = "Project",
                Url = Url.Action("Index", "Project") ?? "#"
            }
        };

        if (id == null || (!Db?.Projects?.Any(x => x.Id == id) ?? false)) 
        {
            return Json("Project not found");
        }

        ViewBag.Categories = Db?.Categories?.ToList();
        ViewBag.Members = Db?.Members?.ToList();

        var project = Db?.Projects?
            .Include(x => x.Members)
            .Include(x => x.Categories)
            .Include(x => x.ProjectImages)
            .FirstOrDefault(x => x.Id == id);
        ViewBag.Project = Newtonsoft.Json.JsonConvert.SerializeObject(project);
        return View(); 
    }

    [Route(BaseUrl + "/projects/{id}")]
    [HttpPost]
    public IActionResult Update(string id, [Bind("Name, Description, Url, Content, Images, MemberIds, CategoryIds")] ProjectInput projectInput)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Project failed to create";
            return BadRequest("Input data is not valid");
        }

        var project = Db?.Projects?
            .Include(x => x.Members)
            .Include(x => x.Categories)
            .Include(x => x.ProjectImages)
            .FirstOrDefault(x => x.Id == id);
        if (project == null)
        {
            TempData["error"] = "Project not found";
            return NotFound("Project not found");
        }

        using var transaction = Db?.Database.BeginTransaction();
        try
        {
            project.Name = projectInput.Name;
            project.Description = projectInput.Description;
            project.Url = projectInput.Url;
            project.Content = projectInput.Content;
            Db?.Projects?.Update(project);

            if (projectInput.MemberIds != null && projectInput.MemberIds.Count > 0)
            {
                var memberIds = projectInput.MemberIds.FirstOrDefault()?.Split(",");
                if (memberIds != null)
                {
                    var deletedMembers = project?.Members?.Where(x => !memberIds.Contains(x.Id)).ToList();
                    var newMembers = memberIds.Where(x => {
                        return project?.Members == null || !project.Members.Any(y => y.Id == x);
                    }).ToList();

                    if (deletedMembers != null && project != null)
                    {
                        foreach(var m in deletedMembers)
                        {
                            var projectMember = Db?.ProjectMembers?.Where(x => x.ProjectId == project.Id && x.MemberId == m.Id).FirstOrDefault();
                            if (projectMember != null)
                            {
                                Db?.ProjectMembers?.Remove(projectMember);
                            }
                        }
                    }

                    if (newMembers != null && project != null)
                    {
                        foreach(var m in newMembers)
                        {
                            var projectMember = new ProjectMember
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectId = project.Id,
                                MemberId = m
                            };
                            Db?.Add(projectMember);
                        }
                    }
                }
            }

            if (projectInput.CategoryIds != null && projectInput.CategoryIds.Count > 0)
            {
                var categoryIds = projectInput.CategoryIds.FirstOrDefault()?.Split(",");
                if (categoryIds != null)
                {
                    var deletedCategories = project?.Categories?.Where(x => !categoryIds.Contains(x.Id)).ToList();
                    var newCategories = categoryIds.Where(x => {
                        return project?.Categories == null || !project.Categories.Any(y => y.Id == x);
                    }).ToList();
                    
                    if (deletedCategories != null && project != null)
                    {
                        foreach (var c in deletedCategories)
                        {
                            var projectCategory = Db?.ProjectCategories?.Where(x => x.ProjectId == project.Id && x.CategoryId == c.Id).FirstOrDefault();
                            if (projectCategory != null)
                            {
                                Db?.ProjectCategories?.Remove(projectCategory);
                            }
                        }
                    }

                    if (newCategories != null && project != null)
                    {
                        foreach (var c in newCategories)
                        {
                            var projectCategory = new ProjectCategory
                            {
                                Id = Guid.NewGuid().ToString(),
                                ProjectId = project.Id,
                                CategoryId = c
                            };
                            Db?.Add(projectCategory);
                        }
                    }
                }
            }

            if (projectInput.Images != null && projectInput.Images.Count > 0)
            {
                if (project?.ProjectImages != null)
                {
                    foreach(var image in project.ProjectImages)
                    {
                        FileController?.DeleteFile(image.Url);
                    }
                    Db?.RemoveRange(project.ProjectImages);
                }

                if (project != null)
                {
                    var images = projectInput.Images.Select(x => new ProjectImage
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProjectId = project.Id,
                        Url = $"{Request.Scheme}://{Request.Host}/storage/{FileController?.UploadFile(x)}"
                    }).ToList();
                    Db?.AddRange(images);
                }
            }
            
            Db?.SaveChanges();
            transaction?.Commit();
            TempData["success"] = "Project successfully updated!";
            return Content(Url.Action("Index", "Project") ?? "");
        }
        catch (Exception e)
        {
            string message = $"Catching error on Update Project: {e.Message}";
            TempData["error"] = "Project failed to update";
            return Json(e);
        }
    }

    [Route(BaseUrl + "/projects/delete/{id}")]
    [HttpPost]
    public IActionResult Delete(string id)
    {
        try
        {
            if (id == null || (!Db?.Projects?.Any(x => x.Id == id) ?? false)) 
            {
                return Json("Project not found");
            }

            var deletedProject = Db?.Projects?.Include(x => x.ProjectImages).FirstOrDefault(x => x.Id == id);
            if (deletedProject == null)
            {
                return Json("Project not found");
            }

            if (deletedProject.ProjectImages?.Count > 0)
            {
                foreach (var image in deletedProject.ProjectImages)
                {
                    FileController?.DeleteFile(image.Url);
                }
            }
            Db?.Projects?.Remove(deletedProject);
            Db?.SaveChanges();

            TempData["success"] = "Project successfully deleted!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            string message = $"Catching error on Delete Project: {e.Message}";
            TempData["error"] = "Project failed to delete";
            return Json(e);
        }
    }

    [Route(BaseUrl + "/projects/{id}/detail")]
    public IActionResult Detail(string id)
    {
        if (id == null || (!Db?.Projects?.Any(x => x.Id == id) ?? false)) 
        {
            return Content("Project not found");
        }

        var project = Db?.Projects?
            .Include(x => x.Members)
            .Include(x => x.Categories)
            .Include(x => x.ProjectImages)
            .FirstOrDefault(x => x.Id == id);
        if (project == null)
        {
            return Content("Project not found");
        }
        
        ViewData["Title"] = $"{project.Name} - Project";
        ViewData["ActiveBreadcrumb"] = "Detail Project";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Title = "Project",
                Url = Url.Action("Index", "Project") ?? "#"
            }
        };
        
        return View(project); 
    }
}
