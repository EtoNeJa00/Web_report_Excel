using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_report_Excel.Models;

namespace Web_report_Excel.ViewModels
{
    public class PageViewModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }

        public PageViewModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                
                return (PageNumber < TotalPages);
            }
        }
    }
}
