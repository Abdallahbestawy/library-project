using Domain.Models;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.DataBase;
using Service.ViewModel;
using System.Data;
using WebLibrary.Controllers;

namespace WebLibrary.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly LibraryContext context;
        public AccountsController(RoleManager<IdentityRole> _roleManager, LibraryContext _context, UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            roleManager = _roleManager;
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
        }
        //[Authorize(Roles = "SuperAdmin")]
        public IActionResult Roles()
        {
            return View(new RolesViewModel
            {
                NewRole = new NewRole(),
                Roles = roleManager.Roles.OrderBy(x => x.Name).ToList()
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Roles(RolesViewModel model)
        {
            if (true)
            {
                // Create
                if (model.NewRole.RoleId == null)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(model.NewRole.RoleName));

                    if (result.Succeeded) //Successeded
                    {
                        SessionMsg(Helper.Success,Resource.ResourceWeb.lbbtnSave,Resource.ResourceWeb.lbSaveMsgRole);
                    }
                    else // Not Successeded
                    {
                        SessionMsg(Helper.Error,Resource.ResourceWeb.lbNotSaved,Resource.ResourceWeb.lbNotSavedMsgRole);
                    }
                } //Update
                else
                {
                    var RoleUpdate = await roleManager.FindByIdAsync(model.NewRole.RoleId);
                    RoleUpdate.Id = model.NewRole.RoleId;
                    RoleUpdate.Name = model.NewRole.RoleName;
                    var Result = await roleManager.UpdateAsync(RoleUpdate);
                    if (Result.Succeeded)
                    {
                        SessionMsg(Helper.Success, Resource.ResourceWeb.lbUpdate,Resource.ResourceWeb.lbUpdateMsgRole);
                    }
                    else // Not Successeded
                    {
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbNotUpdate, Resource.ResourceWeb.lbNotUpdateMsgRole);
                    }
                }
            }
            return RedirectToAction("Roles");
        }

        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = roleManager.Roles.FirstOrDefault(x => x.Id == Id);
            if ((await roleManager.DeleteAsync(role)).Succeeded)
                return RedirectToAction(nameof(Roles));

            return RedirectToAction("Roles");
        }
        public IActionResult Registers()
        {
            var model = new RegisterViewModel
            {
                NewRegister = new NewRegister(),
                Roles = roleManager.Roles.OrderBy(d => d.Name).ToList(),
                Users = context.VwUsers.OrderBy(d => d.Role).ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registers(RegisterViewModel model)
        {
            if (true)
            {
                var file = HttpContext.Request.Form.Files;
                if (file.Count > 0)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                    var fileStream=new FileStream(Path.Combine(@"wwwroot",Helper.PathSaveImageUser,ImageName), FileMode.Create);
                    file[0].CopyTo(fileStream);
                    model.NewRegister.ImageUser = ImageName;
                }
                else
                {
                    model.NewRegister.ImageUser = model.NewRegister.ImageUser;
                }
                var user = new ApplicationUser 
                {
                    Id= model.NewRegister.Id,
                    Name=model.NewRegister.Name,
                    ImageUser=model.NewRegister.ImageUser,
                    Email=model.NewRegister.Email,
                    UserName=model.NewRegister.Email,
                    ActiveUser=model.NewRegister.ActiveUser,
                };
                // create
                if (user.Id == null)
                {
                    user.Id = Guid.NewGuid().ToString();
                    var result = await userManager.CreateAsync(user, model.NewRegister.Password);
                    if (result.Succeeded)
                    {
                        var Role = await userManager.AddToRoleAsync(user,model.NewRegister.RoleName);
                        if (Role.Succeeded)
                        {
                            SessionMsg(Helper.Success,Resource.ResourceWeb.lbbtnSave,Resource.ResourceWeb.lbSavedMsgUserRole);
                        }
                        else
                        {
                            SessionMsg(Helper.Error, Resource.ResourceWeb.lbbtnSave, Resource.ResourceWeb.lbNotSavedMsgUser);

                        }
                    }
                    else
                    {
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbbtnSave, Resource.ResourceWeb.lbNotSaveNewUser);
                    }
                }
                // update
                else
                {
                    var userUpdate=await userManager.FindByIdAsync(user.Id);
                    userUpdate.Id = model.NewRegister.Id;
                    userUpdate.Name=model.NewRegister.Name;
                    userUpdate.Email = model.NewRegister.Email;
                    userUpdate.ActiveUser=model.NewRegister.ActiveUser;
                    userUpdate.UserName = model.NewRegister.Email;
                    userUpdate.ImageUser=model.NewRegister.ImageUser;
                    var result=await userManager.UpdateAsync(userUpdate);
                    if (result.Succeeded)
                    {
                        var oldRole = await userManager.GetRolesAsync(userUpdate);
                        await userManager.RemoveFromRolesAsync(userUpdate, oldRole);
                        var NewRole = await userManager.AddToRoleAsync(userUpdate,model.NewRegister.RoleName);
                        if (NewRole.Succeeded)
                        {
                            SessionMsg(Helper.Success, Resource.ResourceWeb.lbbtnSave, Resource.ResourceWeb.lbSavedMsgUserRole);
                        }
                        else
                        {
                            SessionMsg(Helper.Error, Resource.ResourceWeb.lbbtnSave, Resource.ResourceWeb.lbNotSavedMsgUser);
                        }
                    }
                    else
                    {
                        SessionMsg(Helper.Error, Resource.ResourceWeb.lbbtnSave, Resource.ResourceWeb.lbNotSaveNewUser);
                    }
                }
                return RedirectToAction("Registers", "Accounts");
            }
            return RedirectToAction("Registers", "Accounts");
        }
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = userManager.Users.FirstOrDefault(u=>u.Id==userId);
            if(user!= null && user.ImageUser != Guid.Empty.ToString())
            {
                var pathImage = Path.Combine(@"wwwroot/", Helper.pathImageUser, user.ImageUser);
                if (System.IO.File.Exists(pathImage))
                {
                    System.IO.File.Delete(pathImage);
                }
                if((await userManager.DeleteAsync(user)).Succeeded)
                {
                    return RedirectToAction("Registers", "Accounts");
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Registers", "Accounts");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(RegisterViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.ChangePassword.Id);
            if (user != null)
            {
                await userManager.RemovePasswordAsync(user);
                var AddNewPassword = await userManager.AddPasswordAsync(user, model.ChangePassword.NewPassword);
                if (AddNewPassword.Succeeded)
                {
                    SessionMsg(Helper.Success, Resource.ResourceWeb.lbbtnSave, Resource.ResourceWeb.lbMsgSavedChangePassword);
                }
                else
                {
                    SessionMsg(Helper.Error, Resource.ResourceWeb.lbbtnSave, Resource.ResourceWeb.lbMsgNotSavedChangePassword);
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction("Registers", "Accounts");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Result = await signInManager.PasswordSignInAsync(model.Eamil, model.Password, model.RememberMy, false);
                if (Result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorLogin = false;
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        private void SessionMsg(string MsgType,string Title,string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, MsgType);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Message, Msg);
        }
    }
}
