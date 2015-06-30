using System.Web.Mvc;
using Common.Data;

namespace Common.Web.Mvc.Entities
{
    public class EntityBase<TKey> : IEntityBase<TKey>
    {
        [HiddenInput(DisplayValue = false)]
        public TKey Id { get; set; }
    }
}