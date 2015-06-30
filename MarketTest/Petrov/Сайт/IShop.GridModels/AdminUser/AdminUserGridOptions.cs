using System;
using System.Collections.Generic;
using Common.Core.Sorting;
using Common.Web.Mvc.Controls;

namespace IShop.GridModels
{
    [Serializable]
    public class AdminUserGridOptions : GridOptions
    {
        public AdminUserGridOptions()
        {
            SortOptions = new List<GridSortOptions>
                {
                    new GridSortOptions { Column = "RoleId", Direction = SortDirection.Ascending },
                    new GridSortOptions { Column = "UserName", Direction = SortDirection.Ascending }
                };
        }
    }
}