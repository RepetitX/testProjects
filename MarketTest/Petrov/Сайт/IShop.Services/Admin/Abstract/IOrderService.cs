using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using Common.Web.Mvc.Controls;
using IShop.GridModels;
using IShop.Models;

namespace IShop.Services
{
    public interface IOrderService
    {
        OrderItem OrderItemsCreate(Guid orderId, IPrincipal principal);
        void OrderItemsCreate(OrderItem entity, IPrincipal principal);
        OrderItem OrderItemsEdit(int id, IPrincipal principal);
        void OrderItemsEdit(OrderItem edit, IPrincipal principal);
        OrderItem OrderItemsBeforeGet(OrderItem entity, IPrincipal principal);
        OrderItem OrderItemsBeforeSave(OrderItem entity, IPrincipal principal);
        void OrderItemsDelete(int key, IPrincipal principal);
        ActionGrid<OrderItem, OrderItemGrid> GetOrderItemsActionGrid(Guid orderId, GridOptions options, IPrincipal principal);

        IEnumerable<SelectListItem> OrderTypeOptions(int productTypeId);
    }
}
