using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_report_Excel.Models;

namespace Web_report_Excel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private DB db;
        public OrderController(DB context)
        {
            db = context;
        }
        [HttpGet("{id}")]
        public Order GetOrder(int id)
        {
            try
            {
                return db.Orders.Where(o => o.Id == id).Single();
            }
            catch{}
            return new Order();
            
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            try
            {
                var orderTmp = new Order()
                {
                    Date = order.Date,
                    PriceSum = order.PriceSum
                };

                db.Orders.Add(orderTmp);
                await db.SaveChangesAsync();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ChengeOrder(int id, Order order)
        {
            try
            {
                var orderDB = db.Orders.Where(o => o.Id == id).Single();
                orderDB.Date = order.Date;
                orderDB.PriceSum = order.PriceSum;
                db.Orders.Update(orderDB);
                await db.SaveChangesAsync();
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(404);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Order orderCheck = db.Orders.Where(o => o.Id == id).Single();
            if (orderCheck != null)
            {
                db.Orders.Remove(orderCheck);
                await db.SaveChangesAsync();
                return StatusCode(200);
            }
            return StatusCode(404);
        }
    }
}
