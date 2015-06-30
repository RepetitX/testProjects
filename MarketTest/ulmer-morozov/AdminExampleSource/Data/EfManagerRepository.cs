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
    public class EfManagerRepository : IManagerRepository
    {
        public async Task<IEnumerable<Manager>> GetMany(Expression<Func<Manager, bool>> selector)
        {
            using (var context = new ApplicationDbContext())
            {
                var managers = await context.Managers.Where(selector).ToListAsync();
                return managers;
            }
        }

        public Task<IEnumerable<Manager>> GetMany()
        {
            return GetMany(x => true);
        }
    }
}