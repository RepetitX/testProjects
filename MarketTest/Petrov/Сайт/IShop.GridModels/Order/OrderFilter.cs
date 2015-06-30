using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Common.Web.Mvc;
using Common.Web.Mvc.Autofac;
using Common.Web.Mvc.Controls;
using IShop.Models;

namespace IShop.GridModels
{
    public class OrderFilter : Filter<Order>
    {
        protected override Filter<Order> Configure()
        {
            #region ProductType

            if (User.IsInRole("Administrator"))
            {
                var userRepository = AutofacLifetimeScope.GetRepository<ApplicationUser, int>();

                var userManagerDictionary = userRepository.GetQuery().ToSelectList(c => c.Id, c => c.UserName, c => false).ToList();

                if (userManagerDictionary.Any())

                    AddCondition(new SelectFilterCondition<Order, string>(c => c.UserManagerId, userManagerDictionary, "Менеджер"));
            }

            #endregion

            #region CreateDateTime

            AddCondition(new DateFilterCondition<Order, DateTime>(c => c.CreateDateTime, "Дата заказа"));

            #endregion

            return this;
        }
    }
}
