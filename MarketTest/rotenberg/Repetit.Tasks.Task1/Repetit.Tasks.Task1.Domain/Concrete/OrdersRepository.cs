using System;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using Repetit.Tasks.Task1.Domain.Abstract;
using Repetit.Tasks.Task1.Domain.Entities;

namespace Repetit.Tasks.Task1.Domain.Concrete
{
    public class OrdersRepository : IOrdersRepository
    {
        private string _connectionString;
        private Table<Order> _ordersTable;
        private Table<Category> _categoriesTable;
        private Table<Product> _productsTable;
        private Table<OrderItem> _itemsTable;
        private Table<OrderOption> _orderOptionsTable;
        private Table<Option> _optionsTable; 
        public OrdersRepository(string connectionString)
        {
            _connectionString = connectionString;
            DataContext dc = new DataContext(connectionString);
            DataLoadOptions dlo=new DataLoadOptions();
            dlo.LoadWith<Order>(order=>order.OrderItems);
            dlo.LoadWith<OrderItem>(orderItem=>orderItem.Product);

            dc.LoadOptions = dlo;
            _ordersTable = dc.GetTable<Order>();
            _categoriesTable = dc.GetTable<Category>();
            _productsTable = dc.GetTable<Product>();
            _itemsTable = dc.GetTable<OrderItem>();
            _orderOptionsTable = dc.GetTable<OrderOption>();
            _optionsTable = dc.GetTable<Option>();
        }

        public IQueryable<Order> Orders
        {
            get { return _ordersTable; }
        }

        public IQueryable<Category> Categories
        {
            get
            {
                return _categoriesTable;
            }
        }

        public IQueryable<OrderOption> OrderOptions
        {
            get { return _orderOptionsTable; }
        } 

        public bool UpdateOrder(Order newOrder)
        {
            var old = _ordersTable.FirstOrDefault(o => o.OrderID == newOrder.OrderID);
            if (null == old) return false;
            old.ManagerUserID = newOrder.ManagerUserID;
            old.UpdatedDate = DateTime.Now;
            old.AdminUserID = newOrder.AdminUserID;
            old.Address = newOrder.Address;
            old.ContactData = newOrder.ContactData;
            old.Comment = newOrder.Comment;
            old.IsDelivered = newOrder.IsDelivered;  
            _ordersTable.Context.SubmitChanges();
            return true;
        }

        public bool AddProduct(int ProductID, int Number, int OrderID)
        {
            var items = _itemsTable.Where(i => i.OrderID == OrderID);
            var p = items.FirstOrDefault(pr => pr.ProductID == ProductID);
            if (p != null)
            {
                p.Number += Number;
                _itemsTable.Context.SubmitChanges();
            }
            else
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("[insert_order_item]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrderID", OrderID);
                        cmd.Parameters.AddWithValue("@ProductID", ProductID);
                        cmd.Parameters.AddWithValue("@Number", Number);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }

          return true;
         }

        public IQueryable<Product> Products
        {
            get { return _productsTable; }
        }

        public IQueryable<OrderItem> OrderItems
        {
            get { return _itemsTable; }
        }

        public bool EditProduct(OrderItem item)
        {
            var itemOld = _itemsTable.FirstOrDefault(it => it.OrderItemID == item.OrderItemID);
            if (itemOld != null)
            {
                itemOld.ProductID = item.ProductID;
                itemOld.Number = item.Number;
                _itemsTable.Context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool DeleteProductFromOrder(int OrderItemID)
        {
            var orderItem = _itemsTable.FirstOrDefault(it => it.OrderItemID == OrderItemID);
            if (null != orderItem)
            {
                var options = _orderOptionsTable.Where(op => op.OrderItemID == OrderItemID);
                if (options.Any())
                {
                    _orderOptionsTable.DeleteAllOnSubmit(options);
                    _orderOptionsTable.Context.SubmitChanges();
                }
                _itemsTable.DeleteOnSubmit(orderItem);
                _itemsTable.Context.SubmitChanges();

                return true;
            }
            return false;
        }

        public bool DeleteOption(int OrderOptionsID)
        {
            var option = _orderOptionsTable.First(op => op.OrderOptionsID == OrderOptionsID);
            _orderOptionsTable.DeleteOnSubmit(option);
            _orderOptionsTable.Context.SubmitChanges();
            return true;
        }

        public bool EditOption(OrderOption option)
        {
            var oldOption = _orderOptionsTable.FirstOrDefault(op => op.OrderOptionsID == option.OrderOptionsID);
            if (null != oldOption)
            {
                oldOption.Number = option.Number;
                _orderOptionsTable.Context.SubmitChanges();
                return true;
            }

            return false;
        }

        public bool AddOptionToItem(int OrderItemID, int OptionID, int Number, int ProductID, int OrderID)
        {

            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("[add_option]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderItemID", OrderItemID);
                    cmd.Parameters.AddWithValue("@OptionID", OptionID);
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters.AddWithValue("@ProductID", ProductID);
                    cmd.Parameters.AddWithValue("@Number", Number);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        public IQueryable<Option> Options
        {
            get { return _optionsTable; }
        }
    }
}
