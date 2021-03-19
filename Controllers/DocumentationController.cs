using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindCore.Models;

namespace NorthwindCore.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        // GET api/info string
        [HttpGet]
        [Route("")]
        public string Document()
        {
            return "Add your keycode as the last endpoint of URL to enter the documentation of this API.";
            // KEYCODE IS "ABC"
        }


        // GET api/Documentation/"keycode"
        [HttpGet]
        [Route("{key}")]
        public ActionResult GetDoc(string key)
        {
            northwindContext context = new northwindContext();

            List<Documentation> docList = (from d in context.Documentation
                                                  where d.Keycode == key
                                                  select d).ToList();
            if (docList.Count > 0)
            {
                return Ok(docList);
            }
            else
            {
                return BadRequest("Antamallasi koodilla ei löydy dokumentaatiota, päiväys: " + DateTime.Now.ToString());
            }

        }
        // GET api/Documentation/"keycode"
        //PublicDocumentation on tietokanta contextista riippumaton oma luokkamääritys,
        //jonka lennosta luotavaan instanssiin sijoitetaan kantaluokasta lennosta poimittavat arvot
        //loopissa rivi kerrallaan.
        [HttpGet]
        [Route("listi/{key}")]
        public List<PublicDocument> Get(string key)
        {
            northwindContext context = new northwindContext();

            List<Documentation> privateDocList = (from d in context.Documentation
                                                  where d.Keycode == key
                                                  select d).ToList();
            //return privateDocList;

            List<PublicDocument> publicDocList = new List<PublicDocument>();

            foreach (Documentation privateDoc in privateDocList)
            {
                PublicDocument publicDoc = new PublicDocument();
                publicDoc.DocumentationId = privateDoc.DocumentationId;
                publicDoc.AvailableRoute = privateDoc.AvailableRoute;
                publicDoc.Method = privateDoc.Method;
                publicDoc.Description = privateDoc.Description;
                publicDocList.Add(publicDoc);
            }
            if (publicDocList.Count == 0)
            {
                PublicDocument publicDoc = new PublicDocument();
                publicDoc.DocumentationId = 0;
                publicDoc.AvailableRoute = DateTime.Now.ToString();
                publicDoc.Method = "Documentation missing";
                publicDoc.Description = "Empty";
                publicDocList.Add(publicDoc);
            }

            return publicDocList;
        }


        [HttpPost] //<--filtteri rajoittaa alapuolella olevan metodin vain POST-pyyntöihin (uuden asian luominen tai lisääminen)
        [Route("")] //<--tyhjä reitinmääritys (ei ole pakko laittaa), eli ei mitään lisättävää reittiin, jolloin
        public string PostCreateNew([FromBody] Documentation doku) //<-- [FromBody] tarkoittaa, että HTTP-pyynnön Body:ssä välitetään JSON-muodossa oleva objekti ,joka om Documentation-tyyppinen customer-niminen
        {
            northwindContext context = new northwindContext(); //Context = Kuten entities muodostettu Scaffold DBContext -tykalulla. Voisi olla myös entiteetti frameworkCore
            context.Documentation.Add(doku);
            context.SaveChanges();

            return doku.DocumentationId.ToString(); //kuittaus Frontille, että päivitys meni oikein --> Frontti voi tsekata, että kontrolleri palauttaa saman id:n mitä käsitteli
        }


        [HttpPut] //<--Rest-terminologian  mukaan tietojen päivitys
        [Route("{key}")]  //{key} aaltosulkujen sisällä on muuttujan nimi, ei ole mitenkään varattu sana! Kaksi paraa:  [Route("{key}/{key2}")] 
        public string PutEdit(int key, [FromBody] Documentation newData) //Key on routessa ja toinen para (dataobjekti) tulee HTTP Bodyssä
        {
            northwindContext context = new northwindContext(); //Context = Kuten entities muodostettu Scaffold DBContext -työkalulla. Voisi olla myös entiteetti frameworkCore
            Documentation dokumentti = context.Documentation.Find(key);

            if (dokumentti != null)
            {
                dokumentti.AvailableRoute = newData.AvailableRoute;
                dokumentti.Method = newData.Method;
                dokumentti.Description = newData.Description;
                dokumentti.Keycode = newData.Keycode;
                context.SaveChanges();
                return "OK";
            }
            else
            {
                return "NOT FOUND";
            }
        }
        [HttpDelete]
        [Route("{key}")]
        public string DeleteSingle(int key)
        {
            northwindContext context = new northwindContext();
            Documentation dokumentti = context.Documentation.Find(key);

            if (dokumentti != null)
            {
                context.Documentation.Remove(dokumentti);
                context.SaveChanges();
                return "DELETED";
            }
            return "NOT FOUND";
        }

    }
}