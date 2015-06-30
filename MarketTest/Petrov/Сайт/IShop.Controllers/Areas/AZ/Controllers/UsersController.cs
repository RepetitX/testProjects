using System;
using System.Web.Mvc;
using IShop.GridModels;
using IShop.Services;
using IShop.ViewModels;
using ControllerBase = Common.Web.Mvc.ControllerBase;

namespace IShop.Controllers.Areas.AZ.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : ControllerBase
    {
        private readonly IAdminUsersService _adminUsersService;

        public UsersController(IAdminUsersService adminUsersService)
        {
            _adminUsersService = adminUsersService;
        }

        public ActionResult Index()
        {
            return View(model: this.ToString());
        }

        public ActionResult Grid(AdminUserGridOptions options)
        {
            return View(_adminUsersService.GetActionGrid(options, User));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(_adminUsersService.Create(User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAdminUserViewModel create)
        {
            if (!ViewData.ModelState.IsValid)
                return View(_adminUsersService.FillDictionaries(create, User));

            try
            {
                _adminUsersService.Create(create, User);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Global", e.Message);

                return View(_adminUsersService.FillDictionaries(create, User));
            }

            return RedirectToAction("Edit", new { id = create.Id });
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            return View(_adminUsersService.Edit(id, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditAdminUserViewModel edit)
        {
            if (!ViewData.ModelState.IsValid)
                return View(_adminUsersService.FillDictionaries(edit, User));

            try
            {
                _adminUsersService.Edit(edit, User);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Global", e.Message);

                return View(_adminUsersService.FillDictionaries(edit, User));
            }

            return RedirectToAction("Edit", new { id = edit.Id });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(string id)
        {
            return ExecuteMethod(() => _adminUsersService.Delete(id, User));
        }

        public override string ToString()
        {
            return "Пользователи";
        }
    }
}