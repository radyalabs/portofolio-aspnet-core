namespace portofolio_aspnet_core.Controllers;

[Authorize]
public class CategoryController : AdminBaseController
{
    [Route(BaseUrl + "/categories")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Categories";
        ViewData["ActiveBreadcrumb"] = "Category";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>();

        var categories = Db?.Categories?.ToList();
        return View(categories);
    }

    [Route(BaseUrl + "/categories/create")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Category";
        ViewData["ActiveBreadcrumb"] = "Create Category";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Title = "Category",
                Url = Url.Action("Index", "Category") ?? "#"
            }
        };

        return View();
    }

    [Route(BaseUrl + "/categories/create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Store([Bind("Name")] Category category)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Category failed to create";
                return Json("Input data is not valid");
            }

            Db?.Add(new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = category.Name
            });
            Db?.SaveChanges();
            TempData["success"] = "Category successfully created!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            string message = $"Catching error on Create Category: {e.Message}";
            TempData["error"] = "Category failed to create";
            return Json(e);
        }
    }

    [Route(BaseUrl + "/categories/{id}")]
    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewData["Title"] = "Edit Category";
        ViewData["ActiveBreadcrumb"] = "Edit Category";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Title = "Category",
                Url = Url.Action("Index", "Category") ?? "#"
            }
        };

        if (id == null || (!Db?.Categories?.Any(x => x.Id == id) ?? false)) 
        {
            return Json("Category not found");
        }

        var category = Db?.Categories?.Find(id);
        return View(category); 
    }

    [Route(BaseUrl + "/categories/{id}")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(string id, [Bind("Name")] Category category)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Category failed to update";
                return Json("Input data is not valid");
            }

            if (id == null || (!Db?.Categories?.Any(x => x.Id == id) ?? false)) 
            {
                return Json("Category not found");
            }

            var updatedCategory = Db?.Categories?.Find(id);
            if (updatedCategory == null)
            {
                return Json("Category not found");
            }

            updatedCategory.Name = category.Name;
            Db?.Update(updatedCategory);
            Db?.SaveChanges();

            TempData["success"] = "Category successfully updated!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            string message = $"Catching error on Update Category: {e.Message}";
            TempData["error"] = "Category failed to update";
            return Json(e);
        }
    }

    [Route(BaseUrl + "/categories/delete/{id}")]
    [HttpPost]
    public IActionResult Delete(string id)
    {
        try
        {
            if (id == null || (!Db?.Categories?.Any(x => x.Id == id) ?? false)) 
            {
                return Json("Category not found");
            }

            var deletedCategory = Db?.Categories?.Find(id);
            if (deletedCategory == null)
            {
                return Json("Category not found");
            }

            Db?.Categories?.Remove(deletedCategory);
            Db?.SaveChanges();

            TempData["success"] = "Category successfully deleted!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            string message = $"Catching error on Delete Category: {e.Message}";
            TempData["error"] = "Category failed to delete";
            return Json(e);
        }
    }
}
