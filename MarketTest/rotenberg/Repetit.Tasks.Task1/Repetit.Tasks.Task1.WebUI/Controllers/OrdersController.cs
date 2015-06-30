using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Repetit.Tasks.Task1.Domain.Abstract;
using Repetit.Tasks.Task1.WebUI.Models;
using Repetit.Tasks.Task1.Domain.Entities;
using Repetit.Tasks.Task1.WebUI.Infrastructure;

namespace Repetit.Tasks.Task1.WebUI.Controllers
{
    [Authorize(Roles="Administrators")]
    public class OrdersController : BaseController
    {
        private readonly IOrdersRepository _repo;

        public OrdersController(IOrdersRepository repo)
        {
            _repo = repo;
        }
     
        public ActionResult Index(int pageSize=10, int page=1)
        {
            var orders = _repo.Orders.Where(o=>!o.IsDelivered).OrderBy(o => o.OrderID).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageData pd = new PageData { TotalItems = _repo.Orders.Count(o => !o.IsDelivered), PageSize = pageSize, Page = page };
        
            return View(new OrdersViewModel { CurrenUser = CurrentUser, Orders = orders, PageInfo = pd });
        }

        public  ViewResult OrderOptions(int OrderID)
        {
            ViewBag.ProductID = new SelectList(_repo.Products.ToList(), "ProductID", "Name");
            var order = _repo.Orders.First(o => o.OrderID == OrderID);
            ViewBag.users =
                new SelectList(
                    HttpContext.GetOwinContext()
                        .GetUserManager<AppUserManager>()
                        .Users.Select(u => new { id = u.Id, u.UserName }), "id", "UserName", order.ManagerUserID);
        
          return  View(new ListItemsModel { Items = _repo.Orders.First(o => o.OrderID == OrderID).OrderItems, OrderID = OrderID, Order=order });
        }

        [HttpPost]
        public ActionResult OrderOptions([Bind(Prefix = "Order")]Order order)
        {
            if (ModelState.IsValid)
            {
                if (_repo.UpdateOrder(order))
                {
                    TempData["message"] = "Заказ успешно отредактирован";
                    return RedirectToAction("OrderOptions",new  {order.OrderID});
                }
            }
            ViewBag.ProductID = new SelectList(_repo.Products.ToList(), "ProductID", "Name");
            order = _repo.Orders.First(o => o.OrderID == order.OrderID);
            ViewBag.users =
                new SelectList(
                    HttpContext.GetOwinContext()
                        .GetUserManager<AppUserManager>()
                        .Users.Select(u => new { id = u.Id, u.UserName }), order.ManagerUserID);

            return View(new ListItemsModel { Items = _repo.Orders.First(o => o.OrderID == order.OrderID).OrderItems, OrderID = order.OrderID, Order = order });
        }

        [HttpPost]
        public ActionResult AddProduct(AddItemModel p)
        {
            string message;
            if (ModelState.IsValid)
            {
                message = _repo.AddProduct(p.ProductID, p.Number, p.OrderID)
                    ? "Продукт успешно добавлен в заказ"
                    : "Произошла ошибка при добавлении продукта в заказ";
            }
            else
                message = "Неверно введены данные";
            TempData["message"] = message;
            return RedirectToAction("OrderOptions", new {p.OrderID});
        }

        public PartialViewResult EditProduct(int OrderItemID)
        {
           
            var item = _repo.OrderItems.FirstOrDefault(it => it.OrderItemID == OrderItemID);
            ViewBag.ProductID = new SelectList(_repo.Products.ToList(), "ProductID", "Name",item!=null?item.ProductID:1);
            return PartialView(item);
        }

        [HttpPost]
        public ActionResult EditProduct(OrderItem item)
        {
            if (ModelState.IsValid)
            {
                if (_repo.EditProduct(item))
                {
                    TempData["message"] = "Продукт успешно отредактирован";
                }
                else
                {
                    TempData["message"] = "Произошла ошибка при редактировании продукав в заказе";
                }
            }
            else
            {
                TempData["message"] = "Неверно введены данные";
            }
            return RedirectToAction("OrderOptions", new { item.OrderID });
        }

        public PartialViewResult ProductOptions(int OrderItemID)
        {
            var orderItem = _repo.OrderItems.FirstOrDefault(it => it.OrderItemID == OrderItemID);
            if (null != orderItem)
            {
                return PartialView(orderItem.OrderOptions);
            }
            return PartialView((object) null);
        }

        public PartialViewResult AddOption(int OrderItemID, int OrderID, int ProductID)
        {
            ViewBag.OptionID = new SelectList(_repo.Options.Where(op => op.ProductID == ProductID), "OptionID", "Name");
            return PartialView(new AddOptionModel { OrderID = OrderID, OrderItemID = OrderItemID, Number = 1 });
        }

        [HttpPost]
        public ActionResult AddOption(AddOptionModel option)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = _repo.AddOptionToItem(option.OrderItemID, option.OptionID, option.Number, option.ProductID, option.OrderID)
                    ? "Продукт успешно отредактирован"
                    : "Произошла ошибка при редактировании продукав в заказе";
            }
            else
            {
                TempData["message"] = "Произошла ошибка при редактировании продукав в заказе";
            }
            return RedirectToAction("OrderOptions", new { option.OrderID });
        }

        public PartialViewResult EditOption(int OrderOptionsID, int ProductID)
        {
            var option = _repo.OrderOptions.First(op => op.OrderOptionsID == OrderOptionsID);
            ViewBag.OptionID = new SelectList(_repo.Options.Where(op => op.ProductID == ProductID), "OptionID", "Name", option.OptionID);
            
            return PartialView(option);
        }

        [HttpPost]
        public ActionResult EditOption(OrderOption option, int OrderID)
        {
            if (ModelState.IsValid)
            {
                _repo.EditOption(option);
                TempData["message"] = "Опция успешно отредактирована";
            }
            else  TempData["message"] = "Вы неверно ввели данные";
            return RedirectToAction("OrderOptions", new {OrderID});
        }

        [HttpPost]
        public ActionResult DeleteProductFromOrder(int OrderItemID, int OrderID)
        {
            TempData["message"] = _repo.DeleteProductFromOrder(OrderItemID) ? "Позиция удалена из заказа" : "Произошла ошибка при выполнении операции";
            return RedirectToAction("OrderOptions", new { OrderID });
        }

        [HttpPost]
        public ActionResult DeleteOption(int OrderOptionsID, int OrderID)
        {
            TempData["message"] = _repo.DeleteOption(OrderOptionsID) ? "Опция удалена из заказа" : "Произошла ошибка при выполнении операции";
            return RedirectToAction("OrderOptions", new { OrderID });
        }
    }
}