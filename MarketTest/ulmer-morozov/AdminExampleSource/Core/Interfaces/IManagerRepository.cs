using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IManagerRepository
    {
        Task<IEnumerable<Manager>> GetMany(Expression<Func<Manager, bool>> selector);
        Task<IEnumerable<Manager>> GetMany();
    }
}