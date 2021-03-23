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

        [HttpGet]
        [Route("{id}")]
        public Logins GetOneUser(int id) // Haku pääavaimella/yksi rivi , Find-metodi hakee AINA VAIN PÄÄAVAIMELLA YHDEN RIVIN
        {
            try
            {
                Logins user = db.Logins.Find(id);
                return user;
            }
            finally
            {
                db.Dispose();
            }
        }

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

        //put eli käyttäjän päivitys
        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateLogin(int id, [FromBody] Logins newLogin)
        {
            //northwindContext db = new northwindContext();
            try
            {
                Logins oldLogin = db.Logins.Find(id);
                if (oldLogin != null)
                {
                    oldLogin.Username = newLogin.Username;
                    oldLogin.Password = newLogin.Password;
                    oldLogin.Firstname = newLogin.Firstname;
                    oldLogin.Lastname = newLogin.Lastname;
                    oldLogin.Email = newLogin.Email;
                    oldLogin.AccesslevelId = newLogin.AccesslevelId;
                 
                    db.SaveChanges();
                    return Ok(newLogin.LoginId);
                }
                else
                {
                    return NotFound("Käyttäjää ei löydy!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Käyttäjän tietojen päivittäminen ei onnistunut.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Yksittäisen käyttäjän poistaminen
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteLogin(int id)
        {
            try
            {
                Logins login = db.Logins.Find(id);
                if (login != null)
                {
                    db.Logins.Remove(login);
                    db.SaveChanges();
                    return Ok("Käyttäjä id:llä " + id + " poistettiin");
                }
                else
                {
                    return NotFound("Käyttäjää id:llä" + id + " ei löydy");
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
