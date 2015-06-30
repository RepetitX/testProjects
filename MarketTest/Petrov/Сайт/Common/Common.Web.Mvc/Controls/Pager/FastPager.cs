using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Common.Core.Pagination;

namespace Common.Web.Mvc.Controls
{
	public class FastPager : Pager
	{
		private readonly IPagination _pagination;
        private readonly HtmlHelper _helper;
	    private readonly IGridOptions _options;

		private string _paginationFormat = Resources.FastPager.PaginationFormat;
        private string _paginationSingleFormat = Resources.FastPager.PaginationSingleFormat;
        private string _paginationFirst = Resources.FastPager.PaginationFirst;
        private string _paginationPrev = Resources.FastPager.PaginationPrev;
        private string _paginationNext = Resources.FastPager.PaginationNext;

        public FastPager(IPagination pagination, IGridOptions options, HtmlHelper helper)
            : base(pagination, helper)
		{
			_pagination = pagination;
            _helper = helper;
            _options = options;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();

			builder.Append("<div class='mvcgrid-pagination'>");

			RenderLeftSideOfPager(builder);

            RenderRightSideOfPager(builder);

			builder.Append(@"</div>");

			return builder.ToString();
		}

		protected virtual void RenderLeftSideOfPager(StringBuilder builder)
		{
			builder.Append("<span class='paginationLeft'>");

			//Special case handling where the page only contains 1 item (ie it's a details-view rather than a grid)
            if (_pagination.PageSize == 1)
            {
                RenderNumberOfItemsWhenThereIsOnlyOneItemPerPage(builder);
            }
            else
            {
                RenderNumberOfItemsWhenThereAreMultipleItemsPerPage(builder);
            }

			builder.Append("</span>");
		}

		protected virtual void RenderRightSideOfPager(StringBuilder builder)
		{
			builder.Append("<span class='paginationRight'>");

		    builder.Append(_options.Page == 1 ? _paginationFirst : CreatePageLink(1, _paginationFirst));

		    builder.Append(" | ");

		    builder.Append(_options.Page > 1 ? CreatePageLink(_options.Page - 1, _paginationPrev) : _paginationPrev);

		    builder.Append(" | ");

		    builder.Append(_pagination.TotalItems == 0 ? _paginationNext : CreatePageLink(_options.Page + 1, _paginationNext));

		    builder.Append("</span>");
		}

		protected virtual void RenderNumberOfItemsWhenThereIsOnlyOneItemPerPage(StringBuilder builder) 
		{
            if (_pagination.TotalItems != 0)
			    builder.AppendFormat(_paginationSingleFormat, _pagination.FirstItem);
		}

		protected virtual void RenderNumberOfItemsWhenThereAreMultipleItemsPerPage(StringBuilder builder) 
		{
            if(_pagination.TotalItems != 0)
                builder.AppendFormat(_paginationFormat, _pagination.FirstItem + ((_options.Page - 1) * _pagination.PageSize), _pagination.LastItem + ((_options.Page - 1) * _pagination.PageSize)); 
		}

		private string CreatePageLink(int pageNumber, string text)
		{
            var dict = _helper.ViewContext.RouteData.Values;
            dict.Remove("options");
            dict["page"] = pageNumber;
            return _helper.RouteLink(text, dict).ToString();
		}
	}
}