using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common.Core.Pagination
{
	public class QueryPagination<T> : IPagination<T>
	{
        public QueryPagination(IQueryable<T> query, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = query.Count();

            var numberToSkip = (PageNumber - 1) * PageSize;

            if (numberToSkip > TotalItems)
                PageNumber = 1;

            Query = query.Skip(numberToSkip).Take(PageSize);
        }

        public IQueryable<T> Query { get; protected set; }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
		    return Query.GetEnumerator();
		}

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public int TotalItems { get; private set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(((double)TotalItems) / PageSize);
            }
        }

        public int FirstItem
        {
            get
            {
                return ((PageNumber - 1) * PageSize) + 1;
            }
        }

        public int LastItem
        {
            get
            {
                return FirstItem + TotalItems - 1;
            }
        }

        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        public bool HasNextPage
        {
            get { return PageNumber < TotalPages; }
        }
	}
}