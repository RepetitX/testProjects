using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Repository
{
    public interface IRepository<TEntity, in TKey> : IDisposable
        where TEntity : class
    {
        string PrimaryKey { get; }

        IQueryable<TEntity> GetQuery();
        IList<TEntity> GetAll();
        Task<IList<TEntity>> GetAllAsync();
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        TEntity Single(TKey key);
        Task<TEntity> SingleAsync(TKey key);
        TEntity Single(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(TKey key);
        Task<TEntity> SingleOrDefaultAsync(TKey key);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity First(TKey key);
        Task<TEntity> FirstAsync(TKey key);
        TEntity First(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(TKey key);
        Task<TEntity> FirstOrDefaultAsync(TKey key);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(params TEntity[] entities);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> update);
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> update);
        void ReferenceLoad(TEntity entity, params string[] references);
        Task ReferenceLoadAsync(TEntity entity, params string[] references);
        void CollectionLoad(TEntity entity, params string[] collections);
        Task CollectionLoadAsync(TEntity entity, params string[] collections);
        void Delete(TEntity entity);
        int Delete(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        void TruncateTable();
        Task TruncateTableAsync();
        void Attach(TEntity entity);
        void SaveChanges();
        Task SaveChangesAsync();

        Expression<Func<TEntity, bool>> ExpPrimaryKeyEquals(TEntity entity);
        Expression<Func<TEntity, bool>> ExpPrimaryKeyEquals(TKey key);
        Expression<Func<TEntity, bool>> ExpPrimaryKeyContainsIn(TKey[] keys);
        Expression<Func<TEntity, TEntity>> ExpUpdateSortOrder(int sortOrder);
    }
}