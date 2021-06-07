using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web_report_Excel.Models;
using Web_report_Excel.ViewModels;

namespace Web_report_Excel.Controllers
{
    public class HomeController : Controller
    {
        private DB db;
        public HomeController(DB context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 10;

            IQueryable<Order> source = db.Orders.AsQueryable();
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Orders = items
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                    return PartialView("SuccessMessage", true);
                }
                catch
                {
                    return PartialView("SuccessMessage", false);
                }
            }
            else
                return PartialView("SuccessMessage", false);

        }
        public ActionResult OrderEdit(int id)
        {
            try
            {
                Order order = db.Orders.Where(o => o.Id == id).Single();
                if (order != null)
                    return PartialView("OrderEdit", order);
            }
            catch { }
            return StatusCode(404);
        }
        [HttpPost]
        public async Task<IActionResult> ChengeOrder(Order order)
        {
            try
            {
                db.Orders.Update(order);
                await db.SaveChangesAsync();
                return PartialView("SuccessMessage", true);
            }
            catch
            {
                return PartialView("SuccessMessage", false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(Order order)
        {
            Order orderCheck = db.Orders.Where(o => o.Id == order.Id).Single();
            if (orderCheck != null)
            {
                db.Orders.Remove(orderCheck);
                await db.SaveChangesAsync();
                return PartialView("SuccessMessage", true);
            }
            return PartialView("SuccessMessage", false);
        }
        [HttpPost]
        public IActionResult DownloadExcel(DateRange dateRange)
        {
            var orders = db.Orders.Where(o =>
            dateRange.Start.HasValue ? dateRange.Start >= o.Date : true
            &&
            dateRange.End.HasValue ? dateRange.End <= o.Date : true
            ).OrderBy(o=>o.Date).ToList().GroupBy(o => o.Date);

            var days = new Day[orders.Count()];    

            int i = 0;
            foreach (var date in orders)
            {
                days[i] = new Day(date);
                i++;
            }
            var ordersDict = new Dictionary<DateTime, Day>();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");

                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Дата";
                worksheet.Cell(currentRow, 2).Value = "Количество заказов с суммой от 0 до 1000";
                worksheet.Cell(currentRow, 3).Value = "Количество заказов с суммой от 1001 до 5000";
                worksheet.Cell(currentRow, 4).Value = "Количество заказов с суммой от 5001";
                currentRow++;
                foreach (var day in days)
                {
                    worksheet.Cell(currentRow, 1).Value = day.Date;
                    worksheet.Cell(currentRow, 2).Value = day.o0_1000;
                    worksheet.Cell(currentRow, 3).Value = day.o1001_5000;
                    worksheet.Cell(currentRow, 4).Value = day.o5001;
                    currentRow++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
         
                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Orders.xlsx");
                }
            }
        }

        class Day
        {
            public DateTime Date;
            public int o0_1000;
            public int o1001_5000;
            public int o5001;
            public Day(IEnumerable<Order> orders)
            {
                Date = orders.First().Date;
                foreach(Order order in orders)
                {
                    if (order.PriceSum >= 0 && order.PriceSum <= 1000)
                    {
                        o0_1000++;
                    }else if(order.PriceSum > 1000 && order.PriceSum <= 5000)
                    {
                        o1001_5000++;
                    }else if(order.PriceSum > 5000)
                    {
                        o5001++;
                    }
                }
            }
        }
    }
}
