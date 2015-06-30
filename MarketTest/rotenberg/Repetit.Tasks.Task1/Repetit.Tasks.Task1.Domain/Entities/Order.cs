using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace Repetit.Tasks.Task1.Domain.Entities
{
    [Table(Name = "Orders")]
   public class Order
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true, AutoSync = AutoSync.OnInsert)]
        public int OrderID { get; set; }

        [Column]
        [Required(ErrorMessage = "Заказчик-обязательное поле")]
        public string Id { get; set; }

        [Column]
        public string AdminUserID { get; set; }

        [Column]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage="Менеджер-обязательное поле")]
        public string ManagerUserID { get; set; }
    
        [Column]
        [System.ComponentModel.DataAnnotations.DataType(DataType.Date)]
        [UIHint("DateTime")]
        [Required(ErrorMessage="Дата заказа-обязательное поле")]
        public DateTime CreatedDate { get; set; }

        [Column]
        public DateTime? UpdatedDate { get; set; }

        private EntityRef<AspNetUser> _user;

        [System.Data.Linq.Mapping.Association(ThisKey = "Id", Storage = "_user", IsForeignKey = true)]
        public AspNetUser User
        {
            get
            {
                return _user.Entity;
            }
            set 
            {
                _user.Entity = value;
                Id = value.Id;
            }
        }

        private EntityRef<AspNetUser> _admin;

        [System.Data.Linq.Mapping.Association(ThisKey = "AdminUserID", Storage = "_admin", IsForeignKey = true)]
        public AspNetUser Admin
        {
            get
            {
                return _admin.Entity;
            }
            set
            {
                _admin.Entity = value;
                AdminUserID = value.Id;
            }
        }

        private EntityRef<AspNetUser> _manager;

        [System.Data.Linq.Mapping.Association(ThisKey = "ManagerUserID", Storage = "_manager", IsForeignKey = true)]
        public AspNetUser Manager
        {
            get
            {
                return _manager.Entity;
            }
            set
            {
                _manager.Entity = value;
                ManagerUserID = value.Id;
            }
        }

        [Column]
        public string Address { get; set; }

        [Column]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage="Контактные данные-обязательное поле")]
        public string ContactData { get; set; }

        [Column]
        public string Comment { get; set; }

        [System.Data.Linq.Mapping.Association(OtherKey = "OrderID")] 
        private EntitySet<OrderItem> _orderItems;

        public IList<OrderItem> OrderItems
        {
            get { return _orderItems.ToList(); }
        }
        [Column]
        public bool IsDelivered { get; set; }
    }
}
