using Microsoft.AspNetCore.Mvc;
using NorthwindCore.Services.Interfaces;

namespace NorthwindCore.Controllers
{
    [Route("nw/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticateService _authenticateService;

        public AuthenticationController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Models.Logins model)
        {
            var user = _authenticateService.Authenticate(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Käyttäjätunus tai salasana on virheellinen" });

            return Ok(user); // Palautus front endiin
        }
    }
}
