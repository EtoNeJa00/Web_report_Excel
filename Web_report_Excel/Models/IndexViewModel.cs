using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_report_Excel.ViewModels;

namespace Web_report_Excel.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public PageViewModel PageViewModel { get; set; }

    }
}
