using System;
using System.Collections.Generic;
using Common.Core.Sorting;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    [Serializable]
    public class OrderGridOptions : GridOptions
    {
        public OrderGridOptions()
        {
            SortOptions = new List<GridSortOptions>
                {
                    new GridSortOptions { Column = "CreateDateTime", Direction = SortDirection.Descending },
                };
        }
    }
}