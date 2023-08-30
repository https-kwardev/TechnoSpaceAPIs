using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TechnoSpaceAPIs.Controllers
{
    [Route("api")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        [Route("authenticate")]
        public IActionResult Authenticate()
        {
            // Initiate Google authentication
            return Challenge(new AuthenticationProperties { RedirectUri = Url.Action("GoogleCallback") }, "Google");
        }

        [Route("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded)
            {
                return BadRequest("Authentication failed");
            }

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var surname = result.Principal.FindFirst(ClaimTypes.Surname)?.Value;


            return Redirect($"https://localhost:44394/WebForm1?email={email}&name={name}&surname={surname}");
        }
    }
}
