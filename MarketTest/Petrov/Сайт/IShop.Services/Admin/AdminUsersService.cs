using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using IShop.AspNetIdentity;
using IShop.DataAccess.Context;
using IShop.GridModels;
using IShop.Helpers;
using IShop.Models;

using IShop.ViewModels;
using EntityFramework.Extensions;
using Common.Repository;
using Common.Web.Mvc;
using Common.Web.Mvc.AspNetIdentity;
using Common.Web.Mvc.Autofac;
using Common.Web.Mvc.Controls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IShop.Services
{
    public class AdminUsersService : IAdminUsersService
    {
        private readonly Lazy<IShopContext> _dbContext;
        private readonly Lazy<DbSet<IdentityUserRole>> _usersRolesDbSet;
        private readonly IAspNetIdentity<IShopContext, ApplicationUser, ApplicationUserManager, RoleManager<IdentityRole>> _aspNetIdentity;

        public AdminUsersService(
            Lazy<IShopContext> dbContext,
            IAspNetIdentity<IShopContext, ApplicationUser, ApplicationUserManager, RoleManager<IdentityRole>> aspNetIdentity)
        {
            _dbContext = dbContext;
            _usersRolesDbSet = new Lazy<DbSet<IdentityUserRole>>(() => _dbContext.Value.Set<IdentityUserRole>());
            _aspNetIdentity = aspNetIdentity;
        }

        public IQueryable<EditAdminUserViewModel> GetQuery(IPrincipal principal)
        {
            var currentUserId = principal.Identity.GetUserId();

            return _aspNetIdentity.GetUsersQuery()
                .Where(u => u.Id != currentUserId)
                .Select(u => new { u.Id, u.UserName, u.Email, u.Roles,  u.LockoutEnabled })
                .ToList()
                .Select(u => new EditAdminUserViewModel
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        RoleId = u.Roles.Select(r => r.RoleId).FirstOrDefault(),
                        Email = u.Email,
                        LockoutEnabled = u.LockoutEnabled
                    })
                .AsQueryable();
        }

        public ActionGrid<EditAdminUserViewModel, AdminUserGrid> GetActionGrid(AdminUserGridOptions options, IPrincipal principal)
        {
            return new ActionGrid<EditAdminUserViewModel, AdminUserGrid>(GetQuery(principal), options, FillRolesForUsers, false, false);
        }

        public EditAdminUserViewModel Get(string id)
        {
            return _aspNetIdentity.GetUsersQuery()
                .Where(u => u.Id == id)
                .Select(u => new { u.Id, u.UserName, u.Email, u.Roles, u.LockoutEnabled })
                .ToList()
                .Select(u => new EditAdminUserViewModel
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        RoleId = u.Roles.Select(r => r.RoleId).FirstOrDefault(),
                        Email = u.Email,
                        LockoutEnabled = u.LockoutEnabled
                    })
                .SingleOrDefault();
        }

        public CreateAdminUserViewModel Create(IPrincipal principal)
        {
            return FillDictionaries(new CreateAdminUserViewModel(), principal) as CreateAdminUserViewModel;
        }

        public void Create(CreateAdminUserViewModel create, IPrincipal principal)
        {
            _aspNetIdentity.CreateUser(new ApplicationUser(create.UserName) { Email = create.Email, EmailConfirmed = true }, create.Password);

            var userId = _aspNetIdentity.GetUsersQuery().Where(u => u.UserName == create.UserName).Select(u => u.Id).Single();

            _usersRolesDbSet.Value.Add(new IdentityUserRole { UserId = userId, RoleId = create.RoleId });

            _dbContext.Value.SaveChanges();

            create.Id = userId;
        }

        public EditAdminUserViewModel Edit(string id, IPrincipal principal)
        {
            return FillDictionaries(Get(id), principal) as EditAdminUserViewModel;
        }

        public void Edit(EditAdminUserViewModel edit, IPrincipal principal)
        {
            if (_aspNetIdentity.GetUsersQuery().Any(u => u.Id == edit.Id && u.UserName.ToLower() != edit.UserName.ToLower()))
            {
                if (_aspNetIdentity.GetUsersQuery().Any(u => u.Id != edit.Id && u.UserName.ToLower() == edit.UserName.ToLower()))
                    throw new Exception("Невозможно сменить имя пользователя. Пользователь с таким именем уже существует.");

                _aspNetIdentity.GetUsersQuery().Where(u => u.Id == edit.Id).Update(u => new ApplicationUser
                    {
                        UserName = edit.UserName,
                        Email = edit.Email,
                        LockoutEnabled = edit.LockoutEnabled
                    });
            }
            else
            {
                _aspNetIdentity.GetUsersQuery().Where(u => u.Id == edit.Id).Update(u => new ApplicationUser
                    {
                        Email = edit.Email,
                        LockoutEnabled = edit.LockoutEnabled
                    });
            }

            _usersRolesDbSet.Value.Where(ur => ur.UserId == edit.Id).Delete();

            _usersRolesDbSet.Value.Add(new IdentityUserRole { UserId = edit.Id, RoleId = edit.RoleId });

            _dbContext.Value.SaveChanges();
        }

        public AdminUserViewModel FillDictionaries(AdminUserViewModel user, IPrincipal principal)
        {
            var roles = new List<SelectListItem>();

            roles.AddRange(_aspNetIdentity.GetRolesQuery()
                .OrderBy(r => r.Name)
                .ToSelectList(r => r.Id, r => r.Name, r => user.RoleId == r.Id));

            user.RolesDictionary = roles;

            return user;
        }

        public void Delete(string id, IPrincipal principal)
        {
            _aspNetIdentity.DeleteUser(id);
        }

        private void FillRolesForUsers(IEnumerable<EditAdminUserViewModel> users)
        {
            var roles = _aspNetIdentity.GetRolesQuery().Select(r => new { r.Id, r.Name }).ToList();

            foreach (var user in users)
            {
                user.RoleName = roles.Where(r => r.Id == user.RoleId).Select(r => r.Name).FirstOrDefault();
            }
        }
    }
}
