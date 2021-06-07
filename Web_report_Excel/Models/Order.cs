using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web_report_Excel.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,3)")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public decimal PriceSum { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
