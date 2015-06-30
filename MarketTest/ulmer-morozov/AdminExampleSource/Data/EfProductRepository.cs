using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;

namespace Data
{
    public class EfProductRepository : IProductRepository
    {
        public async Task<IEnumerable<Product>> GetMany(Expression<Func<Product, bool>> selector)
        {
            using (var context = new ApplicationDbContext())
            {
                var products = await context.Products
                    .Include(x => x.Accessorises)
                    .Where(selector).ToListAsync();
                return products;
            }
        }

        public Task<IEnumerable<Product>> GetMany()
        {
            return GetMany(x => true);
        }

        public async Task<Product> GetOne(int id)
        {
            using (var context = new ApplicationDbContext())
            {
                var product = await context.Products
                    .Include(x => x.Accessorises)
                    .SingleOrDefaultAsync(x => x.Id == id);
                return product;
            }
        }
    }
}