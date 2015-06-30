using System.Linq;

namespace Common.Web.Mvc.Controls
{
    public interface IFilterCondition
    {
        string Key { get; }
        string Caption { get; }
        FilterConditionValue Value { get; set; }
    }

    public interface IFilterCondition<TEntity> : IFilterCondition
        where TEntity : class
    {
        IQueryable<TEntity> Apply(IQueryable<TEntity> query);
    }
}