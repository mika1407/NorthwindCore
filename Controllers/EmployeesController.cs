using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NorthwindCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// ---------- EMPLOYEES --------------

namespace NorthwindCore.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("nw/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        //Get all Employees -puhdas listaversio-
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("")]
        public List<Employees> GetAllEmp()
        {
            northwindContext db = new northwindContext();
            try {
                List<Employees> emp = db.Employees.ToList();
                //var emp = db.Employees.AsNoTracking().ToList();
                return emp;
            }
            finally
            {
                db.Dispose();
            }
        }


        //GET ALL -  ACTIONRESULT VERSIO
        //public ActionResult GetAllEmployees()
        //{

        //    northwindContext db = new northwindContext();
        //    List<Employees> emp = db.Employees.ToList();

        //    if (emp != null)
        //    {
        //        return Ok(emp);
        //    }
        //    else
        //    {
        //        return NotFound("Sorry, nothing to show.");
        //    }
        //}

        // Haku id:llä
        [HttpGet]
        [Route("{id}")]
        public Employees GetOneEmployee(int id)
        {
            northwindContext db = new northwindContext();
            try
            {
                Employees Employee = db.Employees.Find(id);
                return Employee;
            }
            finally
            {
                db.Dispose();
            }
        }

        // Haku maan nimellä
        [HttpGet]
        [Route("Country/{key}")]
        public List<Employees> GetEmployeesByCountry(string key)
        {
            northwindContext db = new northwindContext();
            try
            {
                var EmpByCountry = from e in db.Employees
                                   where e.Country == key
                                   select e;
                return EmpByCountry.ToList();
            }
            finally
            {
                db.Dispose();
            }
        }

        // Uuden luonti
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("")]
        public ActionResult CreateNewEmployee([FromBody] Employees Employee)
        {
            northwindContext db = new northwindContext();
            try
            {

                db.Employees.Add(Employee);
                db.SaveChanges();
                return Ok(Employee.EmployeeId);
            }
            catch (Exception e)
            {
                return BadRequest("Adding Employee failed" + e);
            }
            finally
            {
                db.Dispose();
            }

        }

        // Olemassaolevan Henkilön päivittäminen
        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateEmployee(int id, [FromBody] Employees newEmp)
        {
            northwindContext db = new northwindContext();
            try
            {
                Employees oldEmp = db.Employees.Find(id);
                if (oldEmp != null)
                {
                    oldEmp.LastName = newEmp.LastName;
                    oldEmp.FirstName = newEmp.FirstName;
                    oldEmp.Title = newEmp.Title;
                    oldEmp.TitleOfCourtesy = newEmp.TitleOfCourtesy;
                    oldEmp.BirthDate = newEmp.BirthDate;
                    oldEmp.HireDate = newEmp.HireDate;
                    oldEmp.Address = newEmp.Address;
                    oldEmp.City = newEmp.City;
                    oldEmp.Region = newEmp.Region;
                    oldEmp.PostalCode = newEmp.PostalCode;
                    oldEmp.Country = newEmp.Country;
                    oldEmp.HomePhone = newEmp.HomePhone;
                    //oldEmp.Extension = newEmp.Extension;
                    //oldEmp.Notes = newEmp.Notes;
                    
                    db.SaveChanges();
                    return Ok(newEmp.EmployeeId);
                }
                else
                {
                    return NotFound("Henkilöä ei loydy!");
                }
            }
            catch
            {
                return BadRequest("henkilön tietojen päivittäminen ei onnistunut.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Yksittäisen henkilön poistaminen
        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            northwindContext db = new northwindContext();
            try
            {
                Employees Employee = db.Employees.Find(id);
                if (Employee != null)
                {
                    db.Employees.Remove(Employee);
                    db.SaveChanges();
                    return Ok("Henkilö id:lla " + id + " poistettiin");
                }
                else
                {
                    return NotFound("Henkilöä id:lla" + id + " ei loydy");
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
