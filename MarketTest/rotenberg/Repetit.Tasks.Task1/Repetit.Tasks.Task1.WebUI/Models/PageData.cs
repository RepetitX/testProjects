using System;
namespace Repetit.Tasks.Task1.WebUI.Models
{
    public class PageData
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public bool HasPreviousPage()
        {
            return Page > 1;
        }

        public bool HasNextPage()
        {
            return Page < Pages;
        }

        public bool IsFirstPage()
        {
            return Page == 1;
        }

        public bool IsLastPage()
        {
            return Page == Pages;
        }

        public int Pages
        {
            get
            {
                return (int)Math.Ceiling(TotalItems * 1.0 / PageSize);
            }
        }
   }
}