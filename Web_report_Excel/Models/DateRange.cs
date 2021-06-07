using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web_report_Excel.Models
{
    public class DateRange
    {
        [Display(Name = "Start Date")]
        public DateTime? Start { get; set; }

        [Display(Name = "End Date")]
        public DateTime? End { get; set; }
    }
}
