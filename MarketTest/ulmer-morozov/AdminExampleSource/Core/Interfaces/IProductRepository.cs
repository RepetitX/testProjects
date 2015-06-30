using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetMany(Expression<Func<Product, bool>> selector);
        Task<IEnumerable<Product>> GetMany();
        Task<Product> GetOne(int id);
    }
}