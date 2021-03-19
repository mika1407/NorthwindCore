using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindCore.Models;

namespace NorthwindCore.Controllers
{
    [Route("nw/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private northwindContext db = new northwindContext();

        //[HttpGet]
        //[Route("{id}")]
        //public Logins GetOneUser(int id) // Haku pääavaimella/yksi rivi , Find-metodi hakee AINA VAIN PÄÄAVAIMELLA YHDEN RIVIN
        //{
        //    northwindContext db = new northwindContext();
        //    Logins user = db.Logins.Find(id);
        //    return user;
        //}

        [HttpGet]
        [Route("")]
        public List<Logins> GetAllUsers() //Hakee kaikki rivit
        {
            northwindContext db = new northwindContext();
            List<Logins> users = db.Logins.ToList();
            return users;
        }


        [HttpGet]
        [Route("LoginID/{key}")]
        public List<Logins> GetUsers(int key) //Hakee jollain tiedolla mätsäävät rivit
        {
            northwindContext db = new northwindContext();

            var Users = from c in db.Logins
                        where c.LoginId == key
                        select c;

            return Users.ToList();
        }

        // Uuden luonti
        [HttpPost]
        [Route("")]
        public ActionResult CreateNewUser([FromBody] Logins user)
        {
            northwindContext db = new northwindContext();
            try
            {

                db.Logins.Add(user);
                db.SaveChanges();
                return Ok(user.LoginId);
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

    }
}
