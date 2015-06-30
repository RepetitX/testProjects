using System;
using System.Web.Mvc;
using Common.Web.Mvc.Controls;
using IShop.GridModels;
using IShop.Models;
using Common.Web.Mvc;
using Common.Web.Mvc.Services;
using IShop.Services;

namespace IShop.Controllers.Areas.AZ.Controllers
{
    [Authorize]
    public class OrderController : FiltrateCRUDController<Order, Guid, OrderGrid, OrderGridOptions, OrderFilter>
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService service)
            : base((IFilterableBaseService<Order, Guid, OrderGrid, OrderGridOptions, OrderFilter>)service)
        {
            _orderService = service;
        }

        [HttpGet]
        public ActionResult EditGeneral(Guid id)
        {
            return View(_service.Edit(id, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGeneral(Order edit)
        {
            return ExecuteSavingMethod<Order, Guid>(edit, () => _service.Edit(edit, User), () => _service.BeforeGet(edit, User));
        }

        public ActionResult OrderItems(Guid orderId)
        {
            return View(model: orderId);
        }

        public ActionResult OrderItemsGrid(Guid orderId)
        {
            return View("Grid", _orderService.GetOrderItemsActionGrid(orderId, new GridOptions(), User));
        }

        [HttpGet]
        public ActionResult OrderItemsCreate(Guid orderId)
        {
            return View(_orderService.OrderItemsCreate(orderId, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderItemsCreate(OrderItem create)
        {
            return ExecuteMethod(() => _orderService.OrderItemsCreate(create, User));
        }

        [HttpGet]
        public ActionResult OrderItemsEdit(int id)
        {
            return View(_orderService.OrderItemsEdit(id, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderItemsEdit(OrderItem create)
        {
            return ExecuteMethod(() => _orderService.OrderItemsEdit(create, User));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual JsonResult OrderItemsDelete(int id)
        {
            return ExecuteMethod(() => _orderService.OrderItemsDelete(id, User));
        }

        [HttpPost]
        public ActionResult OrderTypeOptions(int orderTypeId)
        {
            return View(_orderService.OrderTypeOptions(orderTypeId));
        }

        public override string ToString()
        {
            return "Заказы";
        }
    }
}