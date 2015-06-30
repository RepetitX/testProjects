using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace Common.Repository
{
    public class BaseRepository<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;
        protected readonly TDbContext DbContext;

        public BaseRepository(TDbContext dbContext)
            : this(dbContext, "Id")
        {
        }

        public BaseRepository(TDbContext dbContext, string primaryKey)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
            PrimaryKey = primaryKey;
        }

        public string PrimaryKey { get; protected set; }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return DbSet;
        }

        public virtual IList<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public virtual TEntity Single(TKey key)
        {
            return DbSet.Single(ExpPrimaryKeyEquals(key));
        }

        public virtual async Task<TEntity> SingleAsync(TKey key)
        {
            return await DbSet.SingleAsync(ExpPrimaryKeyEquals(key));
        }

        public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Single(predicate);
        }

        public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.SingleAsync(predicate);
        }

        public virtual TEntity SingleOrDefault(TKey key)
        {
            return DbSet.SingleOrDefault(ExpPrimaryKeyEquals(key));
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(TKey key)
        {
            return await DbSet.SingleOrDefaultAsync(ExpPrimaryKeyEquals(key));
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.SingleOrDefault(predicate);
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.SingleOrDefaultAsync(predicate);
        }

        public virtual TEntity First(TKey key)
        {
            return DbSet.First(ExpPrimaryKeyEquals(key));
        }

        public virtual async Task<TEntity> FirstAsync(TKey key)
        {
            return await DbSet.FirstAsync(ExpPrimaryKeyEquals(key));
        }

        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.First(predicate);
        }

        public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstAsync(predicate);
        }

        public virtual TEntity FirstOrDefault(TKey key)
        {
            return DbSet.FirstOrDefault(ExpPrimaryKeyEquals(key));
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(TKey key)
        {
            return await DbSet.FirstOrDefaultAsync(ExpPrimaryKeyEquals(key));
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual TEntity Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            return DbSet.Add(entity);
        }

        public virtual IEnumerable<TEntity> AddRange(params TEntity[] entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentNullException("entities");

            return DbSet.AddRange(entities);
        }

        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var updateEntity = Single(ExpPrimaryKeyEquals(entity));

            DbContext.Entry(updateEntity).CurrentValues.SetValues(entity);

            return updateEntity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var updateEntity = await SingleAsync(ExpPrimaryKeyEquals(entity));

            DbContext.Entry(updateEntity).CurrentValues.SetValues(entity);

            return updateEntity;
        }

        public virtual int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> update)
        {
            return DbSet.Where(predicate).Update(update);
        }

        public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> update)
        {
            return await DbSet.Where(predicate).UpdateAsync(update);
        }

        public virtual void ReferenceLoad(TEntity entity, params string[] references)
        {
            foreach (var reference in references)
                DbContext.Entry(entity).Reference(reference).Load();
        }

        public virtual async Task ReferenceLoadAsync(TEntity entity, params string[] references)
        {
            foreach (var reference in references)
                await DbContext.Entry(entity).Reference(reference).LoadAsync();
        }

        public virtual void CollectionLoad(TEntity entity, params string[] collections)
        {
            foreach (var collection in collections)
                DbContext.Entry(entity).Collection(collection).Load();
        }

        public virtual async Task CollectionLoadAsync(TEntity entity, params string[] collections)
        {
            foreach (var collection in collections)
                await DbContext.Entry(entity).Collection(collection).LoadAsync();
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DbSet.Remove(entity);
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).Delete();
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).DeleteAsync();
        }

        public virtual void TruncateTable()
        {
            DbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE " + GetTableName());
        }

        public virtual async Task TruncateTableAsync()
        {
            await DbContext.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE " + GetTableName());
        }

        public virtual void Attach(TEntity entity)
        {
            DbSet.Attach(entity);
        }

        public virtual void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                    DbContext.Dispose();
            }
        }

        public Expression<Func<TEntity, bool>> ExpPrimaryKeyEquals(TEntity entity)
        {
            var keyPropertyInfo = typeof(TEntity).GetProperty(PrimaryKey);
            var keyValue = (TKey)keyPropertyInfo.GetValue(entity, null);

            return ExpPrimaryKeyEquals(keyValue);
        }

        public Expression<Func<TEntity, bool>> ExpPrimaryKeyEquals(TKey key)
        {
            var parameter = Expression.Parameter(typeof(TEntity));

            var body = Expression.Equal(Expression.Property(parameter, PrimaryKey), Expression.Constant(key));

            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        public Expression<Func<TEntity, bool>> ExpPrimaryKeyContainsIn(TKey[] keys)
        {
            var parameter = Expression.Parameter(typeof(TEntity));

            var pKeyProperty = Expression.Property(parameter, PrimaryKey);

            var containsMethod = typeof(Enumerable).GetMethods()
                .Where(m => m.Name == "Contains")
                .Single(m => m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TKey));

            var body = Expression.Call(null, containsMethod, Expression.Constant(keys), pKeyProperty);

            return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
        }

        public Expression<Func<TEntity, TEntity>> ExpUpdateSortOrder(int sortOrder)
        {
            var parameter = Expression.Parameter(typeof(TEntity));

            var newEntity = Expression.New(typeof(TEntity));

            var sortOrderMember = typeof(TEntity).GetMember("SortOrder")[0];

            var sortOrderMemberBinding = Expression.Bind(sortOrderMember, Expression.Constant(sortOrder));

            var body = Expression.MemberInit(newEntity, sortOrderMemberBinding);

            return Expression.Lambda<Func<TEntity, TEntity>>(body, parameter);
        }

        protected string GetTableName()
        {
            var sql = ((IObjectContextAdapter)DbContext).ObjectContext.CreateObjectSet<TEntity>().ToTraceString();
            var regex = new Regex("FROM (?<table>.*) AS");
            var match = regex.Match(sql);

            return match.Groups["table"].Value;
        }
    }
}
