namespace portofolio_aspnet_core.Controllers;

[Authorize]
public class MemberController : AdminBaseController
{
    [Route(BaseUrl + "/members")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Members";
        ViewData["ActiveBreadcrumb"] = "Member";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>();
        
        var members = Db?.Members?.ToList();
        return View(members);
    }

    [Route(BaseUrl + "/members/create")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Member";
        ViewData["ActiveBreadcrumb"] = "Create Member";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Title = "Member",
                Url = Url.Action("Index", "Member") ?? "#"
            }
        };

        return View();
    }

    [Route(BaseUrl + "/members/create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Store([Bind("Name", "Description", "Image")] MemberInput member)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Member failed to create";
                return Json("Input data is not valid");
            }

            Db?.Add(new Member
            {
                Id = Guid.NewGuid().ToString(),
                Name = member.Name,
                Description = member.Description,
                ProfilePicture = $"{Request.Scheme}://{Request.Host}/storage/{FileController?.UploadFile(member.Image)}"
            });
            Db?.SaveChanges();
            TempData["success"] = "Member successfully created!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            string message = $"Catching error on Create Member: {e.Message}";
            TempData["error"] = "Member failed to create";
            return Json(e);
        }
    }

    [Route(BaseUrl + "/members/{id}")]
    [HttpGet]
    public IActionResult Edit(string id)
    {
        ViewData["Title"] = "Edit Member";
        ViewData["ActiveBreadcrumb"] = "Edit Member";
        ViewData["Breadcrumbs"] = new List<Breadcrumb>
        {
            new Breadcrumb
            {
                Title = "Member",
                Url = Url.Action("Index", "Member") ?? "#"
            }
        };

        if (id == null || (!Db?.Members?.Any(x => x.Id == id) ?? false)) 
        {
            return Json("Member not found");
        }

        var member = Db?.Members?.Find(id);
        return View(member); 
    }

    [Route(BaseUrl + "/members/{id}")]
    [HttpPost]
    public IActionResult Update(string id, [Bind("Name, Description, Image")] MemberInput memberInput)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Member failed to update";
                return Json("Input data is not valid");
            }

            if (id == null || (!Db?.Members?.Any(x => x.Id == id) ?? false)) 
            {
                return Json("Member not found");
            }

            var updatedMember = Db?.Members?.Find(id);
            if (updatedMember == null)
            {
                return Json("Member not found");
            }

            updatedMember.Name = memberInput.Name;
            updatedMember.Description = memberInput.Description;
            if (memberInput.Image != null)
            {
                if (!string.IsNullOrEmpty(updatedMember.ProfilePicture))
                {
                    FileController?.DeleteFile(updatedMember.ProfilePicture);
                }
                updatedMember.ProfilePicture = $"{Request.Scheme}://{Request.Host}/storage/{FileController?.UploadFile(memberInput.Image)}";
            }
            Db?.Update(updatedMember);
            Db?.SaveChanges();

            TempData["success"] = "Member successfully updated!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            string message = $"Catching error on Create Member: {e.Message}";
            TempData["error"] = "Member failed to create";
            return Json(e);
        }
    }

     [Route(BaseUrl + "/members/delete/{id}")]
    [HttpPost]
    public IActionResult Delete(string id)
    {
        try
        {
            if (id == null || (!Db?.Members?.Any(x => x.Id == id) ?? false)) 
            {
                return Json("Member not found");
            }

            var deletedMember = Db?.Members?.Find(id);
            if (deletedMember == null)
            {
                return Json("Member not found");
            }

            if (!string.IsNullOrEmpty(deletedMember.ProfilePicture))
            {
                FileController?.DeleteFile(deletedMember.ProfilePicture);
            }

            Db?.Members?.Remove(deletedMember);
            Db?.SaveChanges();

            TempData["success"] = "Member successfully deleted!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            string message = $"Catching error on Delete Member: {e.Message}";
            TempData["error"] = "Member failed to delete";
            return Json(e);
        }
    }
}
