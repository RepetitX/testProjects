using IShop.DataAccess.Context;
using Common.Repository;

namespace IShop.DataAccess.Repository
{
    public class IShopRepository<TEntity, TKey> : BaseRepository<IShopContext, TEntity, TKey>
        where TEntity : class
    {
        public IShopRepository(IShopContext context)
            : base(context)
        {
        }

        public IShopRepository(IShopContext context, string keyName)
            : base(context, keyName)
        {
        }
    }
}