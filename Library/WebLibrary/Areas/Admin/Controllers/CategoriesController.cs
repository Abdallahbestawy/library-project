using Domain.IRepositoryService;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.ViewModel;

namespace WebLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IServicesRepository<Category> servicesCategory;
        private readonly IServicesRepositoryLog<LogCategory> servicesLogCategory;
        private readonly UserManager<ApplicationUser> userManager;
        public CategoriesController(IServicesRepository<Category> _servicesCategory, IServicesRepositoryLog<LogCategory> _servicesLogCategory,UserManager<ApplicationUser> _userManager)
        {
            servicesCategory = _servicesCategory;
            servicesLogCategory = _servicesLogCategory;
            userManager = _userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Categories()
        {
            var model = new CategoryViewModel
            {
                Categories= await servicesCategory.GetAll(),
                LogCategories= await servicesLogCategory.GetAll(),
                NewCategory=new Category()
            };
            return View(model);
        }
        public async Task<IActionResult> Delete(Guid Id)
        {
            var userId = userManager.GetUserId(User);
            await servicesCategory.Delete(Id);
            await servicesLogCategory.Delete(Id, Guid.Parse(userId));
            return RedirectToAction("Categories");
        }
        public async Task<IActionResult> DeleteLog(Guid Id)
        {
            if (await servicesLogCategory.DeleteLog(Id))
            {
                return RedirectToAction("Categories");
            }
            return RedirectToAction("Categories");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Save(CategoryViewModel model)
        {
            if (true)
            {
                var userId=userManager.GetUserId(User);
                if (model.NewCategory.Id.Equals(Guid.Parse(Guid.Empty.ToString())))
                {
                    if (await servicesCategory.FindBy(model.NewCategory.Name) != null)
                    {
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgDuplicateNameCategory);
                    }
                    else
                    {
                        if (await servicesCategory.Save(model.NewCategory) && await servicesLogCategory.Save(model.NewCategory.Id, Guid.Parse(userId)))
                        {
                            SessionMsg(Helper.Success, Resource.ResourceWeb.lbSave, Resource.ResourceWeb.lbMsgSaveCategory);
                        }
                        else
                        {
                            SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotSavedCategory);
                        }
                    }
                }
                else
                {
                    if(await servicesCategory.Save(model.NewCategory) &&await servicesLogCategory.Update(model.NewCategory.Id, Guid.Parse(userId)))
                    {
                        SessionMsg(Helper.Success, Resource.ResourceWeb.lbUpdate, Resource.ResourceWeb.lbMsgUpdateCategory);
                    }
                    else
                    {
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotSaved, Resource.ResourceWeb.lbMsgNotUpdatedCategory);
                    }
                }
               
            }
            return RedirectToAction("Categories");
        }
        private void SessionMsg(string MsgType, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, MsgType);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Message, Msg);
        }
    }
}
