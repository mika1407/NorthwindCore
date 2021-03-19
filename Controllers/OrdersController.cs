using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// ----------------- Orders ---------------------

namespace NorthwindCore.Controllers
{
    // Koko tilaustaulun sisältö
    [Route("nw/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Orders> GetAllOrders()
        {
            northwindContext db = new northwindContext();
            try
            {
                List<Orders> Orders = db.Orders.ToList();
                return Orders;
            }
            finally
            {
                db.Dispose();
            }
        }

        // Haku tilaus id:llä
        [HttpGet]
        [Route("{id}")]
        public Orders GetOneorder(int id)
        {
            northwindContext db = new northwindContext();
            try
            {
                Orders order = db.Orders.Find(id);
                return order;
            }
            finally
            {
                db.Dispose();
            }
        }

        // Tilaukset asiakas id:n mukaan
       [HttpGet]
        [Route("CustomerId/{key}")]
        public List<Orders> GetOrdersByCustomer(string key)
        {
            northwindContext db = new northwindContext();
            try
            {
                var orderByCust = from o in db.Orders
                                 where o.CustomerId == key
                                 select o;

                return orderByCust.ToList();
            }
            finally
            {
                db.Dispose();
            }
        }


        // Uuden tilauksen luonti
        [HttpPost]
        [Route("")]
        public ActionResult CreateNeworder([FromBody] Orders order)
        {
            northwindContext db = new northwindContext();
            try
            {

                db.Orders.Add(order);
                db.SaveChanges();
                
                return Ok(order.OrderId);
            }
            catch
            {
                return BadRequest("Adding order failed");
            }
            finally
            {
                db.Dispose();
            }

        }

        // Olemassaolevan tilauksen päivittäminen
        [HttpPut]
        [Route("{id}")]
        public ActionResult Updateorder(int id, [FromBody] Orders newOrder)
        {
            northwindContext db = new northwindContext();
            try
            {
                Orders oldOrder = db.Orders.Find(id);
                if (oldOrder != null)
                {
                    oldOrder.CustomerId = newOrder.CustomerId;
                    oldOrder.EmployeeId = newOrder.EmployeeId;
                    oldOrder.OrderDate = newOrder.OrderDate;
                    oldOrder.RequiredDate = newOrder.RequiredDate;
                    oldOrder.ShippedDate = newOrder.ShippedDate;
                    oldOrder.ShipVia = newOrder.ShipVia;
                    oldOrder.Freight = newOrder.Freight;
                    oldOrder.ShipName = newOrder.ShipName;
                    oldOrder.ShipAddress = newOrder.ShipAddress;
                    oldOrder.ShipCity = newOrder.ShipCity;
                    oldOrder.ShipRegion = newOrder.ShipRegion;
                    oldOrder.ShipPostalCode = newOrder.ShipPostalCode;
                    oldOrder.ShipCountry = newOrder.ShipCountry;
                    oldOrder.Employee = newOrder.Employee;
                    oldOrder.ShipViaNavigation = newOrder.ShipViaNavigation;


                    db.SaveChanges();
                    return Ok(newOrder.OrderId);
                }
                else
                {
                    return NotFound("Tilausta ei löydy!");
                }
            }
            catch
            {
                return BadRequest("Tilauksen tietojen päivittäminen ei onnistunut.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Yksittäisen tilauksen poistaminen
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Deleteorder(int id)
        {
            northwindContext db = new northwindContext();
            try
            {
                Orders order = db.Orders.Find(id);
                if (order != null)
                {
                    db.Orders.Remove(order);
                    db.SaveChanges();
                    return Ok("Tilaus id:llä " + id + " poistettiin");
                }
                else
                {
                    return NotFound("Tilausta id:llä" + id + " ei löydy");
                }
            }
            catch
            {
                return BadRequest();
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
