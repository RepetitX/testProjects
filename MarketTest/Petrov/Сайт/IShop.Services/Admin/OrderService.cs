using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Web.Mvc;
using Common.Repository;
using Common.Web.Mvc;
using Common.Web.Mvc.Autofac;
using Common.Web.Mvc.Controls;
using Common.Web.Mvc.Services;
using IShop.GridModels;
using IShop.Models;

namespace IShop.Services
{
    public class OrderService : FilterableBaseService<Order, Guid, OrderGrid, OrderGridOptions, OrderFilter>, IOrderService
    {
        private readonly Lazy<IRepository<ApplicationUser, string>> _userRepository;
        private readonly Lazy<IRepository<OrderItem, int>> _orderItemsRepository;
        private readonly Lazy<IRepository<ProductOption, int>> _productOptionRepository;

        public OrderService(Lazy<IRepository<Order, Guid>> repository, Lazy<IRepository<ApplicationUser, string>> userRepository, Lazy<IRepository<OrderItem, int>> orderItemsRepository, Lazy<IRepository<ProductOption, int>> productOptionRepository)
            : base(repository)
        {
            _userRepository = userRepository;
            _orderItemsRepository = orderItemsRepository;
            _productOptionRepository = productOptionRepository;
        }

        public override IQueryable<Order> GetQuery(IPrincipal principal)
        {
            if (!principal.IsInRole("Administrator"))
            {
                return _repository.Value.GetQuery().Where(c => c.UserManager.UserName == principal.Identity.Name);
            }
            return _repository.Value.GetQuery();
        }

        public override void Create(Order entity, IPrincipal principal)
        {
            entity.Id = Guid.NewGuid();
            entity.CreateDateTime = DateTime.Now;

            base.Create(entity, principal);
        }

        public override Order BeforeGet(Order entity, IPrincipal principal)
        {
            if (principal.IsInRole("Administrator"))
            {
                entity.UserManagerDictionary = _userRepository.Value.GetQuery().ToSelectList(c => c.Id, c => c.UserName, c => c.UserName == principal.Identity.Name);
            }
            else
            {
                var user = _userRepository.Value.GetQuery().FirstOrDefault(c => c.Id == entity.UserManagerId);

                entity.UserManagerName = user != null ? user.UserName : "Не задан";
            }

            return entity;
        }

        public ActionGrid<OrderItem, OrderItemGrid> GetOrderItemsActionGrid(Guid orderId, GridOptions options, IPrincipal principal)
        {
            var order = GetQuery(principal).FirstOrDefault(c => c.Id == orderId);

            if (order == null)
                throw new Exception("Заказ не найден");

            var query = order.OrderItems.AsQueryable();

            return new ActionGrid<OrderItem, OrderItemGrid>(query, options, false, false);
        }

        public OrderItem OrderItemsCreate(Guid orderId, IPrincipal principal)
        {
            var result = new OrderItem
            {
                OrderId = orderId,
            };

            return OrderItemsBeforeGet(result, principal);
        }

        public void OrderItemsCreate(OrderItem entity, IPrincipal principal)
        {
            var order = _repository.Value.GetQuery().FirstOrDefault(c => c.Id == entity.OrderId);

            if (order == null)
                throw new Exception("Заказ не найден");

            order.OrderItems.Add(OrderItemsBeforeSave(entity, principal));
            _repository.Value.SaveChanges();
        }

        public OrderItem OrderItemsEdit(int id, IPrincipal principal)
        {
            var entity = _orderItemsRepository.Value.FirstOrDefault(c => c.Id == id);

            if (entity == null)
                throw new Exception("Позиция в заказе не найдена");

            return OrderItemsBeforeGet(entity, principal); ;
        }

        public void OrderItemsEdit(OrderItem edit, IPrincipal principal)
        {
            var entity = _orderItemsRepository.Value.FirstOrDefault(c => c.Id == edit.Id);

            if (entity == null)
                throw new Exception("Позиция в заказе не найдена");

            entity.ProductOptionList = edit.ProductOptionList;

            _orderItemsRepository.Value.Update(OrderItemsBeforeSave(entity, principal));

            _orderItemsRepository.Value.SaveChanges();
        }

        public OrderItem OrderItemsBeforeGet(OrderItem entity, IPrincipal principal)
        {
            var result = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = "",
                        Text = ""
                    }
                };

            var existOptions = entity.ProductOptions != null ? entity.ProductOptions.ToList() : new List<ProductOption>();

            result.AddRange(_productOptionRepository.Value.GetQuery().Where(c => c.ProductTypeId == entity.ProductTypeId).ToSelectList(po => po.Id, po => po.Name, po => existOptions.Any(c => c.Id == po.Id)));

            entity.ProductOptionDictionary = result;

            return entity;
        }

        public OrderItem OrderItemsBeforeSave(OrderItem entity, IPrincipal principal)
        {
            entity.ProductOptions.Clear();

            if (entity.ProductOptionList.Any())
            {
                var productOptions = _productOptionRepository.Value.GetQuery().Where(c => entity.ProductOptionList.Contains(c.Id) && c.ProductTypeId == entity.ProductTypeId).ToList();

                entity.ProductOptions.Clear();

                _productOptionRepository.Value.SaveChanges();

                entity.ProductOptions = productOptions;
            }

            return entity;
        }

        public IEnumerable<SelectListItem> OrderTypeOptions(int productTypeId)
        {
            return _productOptionRepository.Value.GetQuery().Where(c => c.ProductTypeId == productTypeId).ToSelectList(c => c.Id, c => c.Name, c => false);
        }

        public void OrderItemsDelete(int key, IPrincipal principal)
        {
            var entity = _orderItemsRepository.Value.FirstOrDefault(c => key == c.Id);

            if (entity == null)
                throw new Exception("Позиция в заказе не найдена");

            _orderItemsRepository.Value.Delete(entity);
            _orderItemsRepository.Value.SaveChanges();
        }
    }
}
