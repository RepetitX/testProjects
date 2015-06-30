using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;

namespace Data
{
    public class EfOrderRepository : IOrderRepository
    {
        public async Task Delete(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbOrder = await context.Orders.SingleOrDefaultAsync(x => x.Id == id);
                context.Orders.Remove(dbOrder);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Order> GetOne(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbOrder = await context.Orders
                    .Include(x => x.Items)
                    .Include(x => x.Manager)
                    .Include(x => x.Items.Select(y => y.Product))
                    .Include(x => x.Items.Select(y => y.Product.Accessorises))
                    .SingleOrDefaultAsync(x => x.Id == id);
                return dbOrder;
            }
        }

        public async Task<IEnumerable<Order>> GetMany(int pageSize, int offset)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbOrders = await context.Orders
                    .Include(x => x.Items)
                    .Include(x => x.Manager)
                    .Include(x => x.Items.Select(y => y.Product))
                    .Include(x => x.Items.Select(y => y.Product.Accessorises))
                    .OrderByDescending(x => x.Id)
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync();
                return dbOrders;
            }
        }

        public async Task<Order> Store(Order newOrder)
        {
            using (var context = new ApplicationDbContext())
            {
                var dbOrder = await context.Orders
                    .Include(x => x.Items)
                    .Include(x => x.Manager)
                    .Include(x => x.Items.Select(y => y.Product))
                    .Include(x => x.Items.Select(y => y.Product.Accessorises))
                    .SingleOrDefaultAsync(x => x.Id == newOrder.Id);
                if (dbOrder == null)
                {
                    //новый заказ
                    var items = newOrder.Items;
                    newOrder.SetCreationDateTime(DateTime.Now);
                    newOrder.SetItems(new List<OrderItem>());
                    if (newOrder.Manager != null)
                    {
                        context.Managers.Attach(newOrder.Manager);
                    }
                    context.Orders.Add(newOrder);
                    await context.SaveChangesAsync();
                    var newItems = items.Select(x => new OrderItem(newOrder.Id, x.Product, x.Count)).ToArray();
                    foreach (var item in newItems)
                    {
                        item.ClearEntity();
                    }
                    context.OrderItems.AddOrUpdate(newItems);
                    newOrder.SetItems(newItems);
                    await context.SaveChangesAsync();
                    return newOrder;
                }
                context.Entry(dbOrder).CurrentValues.SetValues(newOrder);
                //Обновим заказы
                //сперва удалим все устаревшие связи на заказы
                foreach (var dbItem in dbOrder.Items.ToList())
                    if (newOrder.Items.All(s => s.ProductId != dbItem.ProductId))
                        context.OrderItems.Remove(dbItem);

                foreach (var newItem in newOrder.Items)
                {
                    var dbItem = dbOrder.Items.SingleOrDefault(s => s.ProductId == newItem.ProductId);
                    if (dbItem != null)
                        //нужно обновить существующий товар в заказе
                        context.Entry(dbItem).CurrentValues.SetValues(newItem);
                    else
                    {
                        //очистим, во избежание дубликатов продукта
                        newItem.ClearEntity();
                        //нужно создать новый заказ в товаре
                        newItem.SetOrderId(newOrder.Id);
                        dbOrder.Items.Add(newItem);
                    }
                }
                //Обновим менеджера
                if (dbOrder.Manager != null)
                {
                    if (newOrder.Manager != null)
                    {
                        if (dbOrder.Manager.Id == newOrder.Manager.Id)
                        {
                            // no relationship change, only scalar prop.
                            context.Entry(dbOrder.Manager).CurrentValues.SetValues(newOrder.Manager);
                        }
                        else
                        {
                            if (newOrder.Manager.Id != 0)//изменился менеджер
                            {
                                // подразумеваем, что новый менеджер уже существует
                                context.Managers.Attach(newOrder.Manager);
                                dbOrder.SetManager(newOrder.Manager);
                            }
                            else
                            {
                                throw new NotImplementedException("Создание менеджера таким образом не поддерживается");
                            }
                        }
                    }
                    else // relationship has been removed
                        dbOrder.SetManager(null);
                }
                else
                {
                    if (newOrder.Manager != null) //изменился менеджер
                    {
                        if (newOrder.Manager.Id != 0)
                        {
                            // подразумеваем, что новый менеджер уже существует
                            context.Managers.Attach(newOrder.Manager);
                            dbOrder.SetManager(newOrder.Manager);
                        }
                        else
                        {
                            throw new NotImplementedException("Создание менеджера таким образом не поддерживается");
                        }
                    }
                    //else ничего не поменялось
                }
                await context.SaveChangesAsync();
                return dbOrder;
            }
        }
    }
}