using Common.Web.Mvc.Entities;

namespace Common.Web.Mvc.Controls.Filter
{
    public class SavedFilter : EntityBase<int>
    {
        public virtual string Name { get; set; }
        public virtual string FilterKey { get; set; }
        public virtual string UserId { get; set; }
        public virtual string GridOptions { get; set; }
    }
}