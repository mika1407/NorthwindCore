using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

// ---------- PRODUCTS --------------

namespace CoreApiHarj.Controllers
{
    // Koko taulun sisältö
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("nw/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        [HttpGet]
        [Route("R")]
        public IActionResult GetSomeProducts(int offset, int limit)
        {
            northwindContext db = new northwindContext();
            try
            {
                    List<Products> tuotteetAnnosteltuna = db.Products.Skip(offset).Take(limit).ToList();
                    return Ok(tuotteetAnnosteltuna);
            }
            catch
            {
                return BadRequest("Something went totally wrong. At first check connections and then if still needed ask software supplier for help.");
            }
            finally
            {
                db.Dispose();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("")]
        public List<Products> GetAllProducts()
        {
            northwindContext db = new northwindContext();
            try
            {
                List<Products> products = db.Products.ToList();
                return products;
            }
            finally
            {
                db.Dispose();
            }
        }

        // Haku id:llä
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("{id}")]
        public Products GetOneProduct(int id)
        {
            northwindContext db = new northwindContext();
            try
            {
                Products product = db.Products.Find(id);
                return product;
            }
            finally
            {
                db.Dispose();
            }
        }

        // Tuotehaku toimittajakoodilla
        [HttpGet]
        [Route("supplierId/{key}")]
        public List<Products> GetProductsBySupplier(int key)
        {
            northwindContext db = new northwindContext();
            try
            {
                var prodBySupp = from p in db.Products
                                 where p.SupplierId == key
                                 select p;

                return prodBySupp.ToList();
            }
            finally
            {
                db.Dispose();
            }
        }

        
        [HttpGet]
        [Route("min-price/{min}/max-price/{max}")]
        public List<Products> GetByPrice(int min, int max)
        {
            northwindContext db = new northwindContext();
            var tuotteet = from p in db.Products
                           where p.UnitPrice > min && p.UnitPrice < max
                            select p;
            return tuotteet.ToList();

        }


        // Uuden luonti
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("")]
        public ActionResult CreateNewProduct([FromBody] Products product)
        {
            northwindContext db = new northwindContext();
            try
            {

                db.Products.Add(product);
                db.SaveChanges();
                return Ok(product.ProductId);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            finally
            {
                db.Dispose();
            }

        }

        // Olemassaolevan tuotteen päivittäminen
        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] Products newProd)
        {
            northwindContext db = new northwindContext();
            try
            {
                Products oldProd = db.Products.Find(id);
                if (oldProd != null)
                {
                    oldProd.ProductName = newProd.ProductName;
                    oldProd.SupplierId = newProd.SupplierId;
                    oldProd.CategoryId = newProd.CategoryId;
                    oldProd.QuantityPerUnit = newProd.QuantityPerUnit;
                    oldProd.UnitPrice = newProd.UnitPrice;
                    oldProd.UnitsInStock = newProd.UnitsInStock;
                    oldProd.UnitsOnOrder = newProd.UnitsOnOrder;
                    oldProd.ReorderLevel = newProd.ReorderLevel;
                    oldProd.Discontinued = newProd.Discontinued;

                    db.SaveChanges();
                    return Ok(newProd.ProductId);
                }
                else
                {
                    return NotFound("Tuotetta ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Tuotteen tietojen päivittäminen ei onnistunut.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Yksittäisen tuotteen poistaminen
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            northwindContext db = new northwindContext();
            try
            {
                Products product = db.Products.Find(id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return Ok("Tuote id:llä " + id + " poistettiin");
                }
                else
                {
                    return NotFound("Tuotetta id:llä" + id + " ei löydy");
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
