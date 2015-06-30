using System;
using System.Collections.Generic;
using Common.Core.Sorting;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    [Serializable]
    public class ProductTypeGridOptions : GridOptions
    {
        public ProductTypeGridOptions()
        {
            SortOptions = new List<GridSortOptions>
                {
                    new GridSortOptions { Column = "Name", Direction = SortDirection.Ascending },
                };
        }
    }
}