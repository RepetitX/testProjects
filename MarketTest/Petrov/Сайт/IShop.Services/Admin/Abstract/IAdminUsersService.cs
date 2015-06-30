using System.Linq;
using System.Security.Principal;
using IShop.GridModels;
using IShop.ViewModels;
using Common.Web.Mvc.Controls;

namespace IShop.Services
{
    public interface IAdminUsersService
    {
        IQueryable<EditAdminUserViewModel> GetQuery(IPrincipal principal);
        ActionGrid<EditAdminUserViewModel, AdminUserGrid> GetActionGrid(AdminUserGridOptions options, IPrincipal principal);
        EditAdminUserViewModel Get(string id);
        CreateAdminUserViewModel Create(IPrincipal principal);
        void Create(CreateAdminUserViewModel create, IPrincipal principal);
        EditAdminUserViewModel Edit(string id, IPrincipal principal);
        void Edit(EditAdminUserViewModel edit, IPrincipal principal);
        AdminUserViewModel FillDictionaries(AdminUserViewModel user, IPrincipal principal);
        void Delete(string id, IPrincipal principal);
    }
}