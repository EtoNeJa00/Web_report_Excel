using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_report_Excel.Models
{
    public class DB:DbContext
    {
        public string ConnectionString { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DB(DbContextOptions<DB> options) : base(options)
        {
        }

    }
}
