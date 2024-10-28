using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogoutController(ISessionService sessionService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationList) || authorizationList.Count == 0)
                throw new NoAuthorizationHeaderException();

            var authorization = authorizationList[0];
            if (string.IsNullOrEmpty(authorization))
                throw new NoAuthorizationHeaderException();

            if (!authorization[..7].Equals("bearer ", StringComparison.CurrentCultureIgnoreCase))
                throw new BadAuthorizationScheme();

            return Ok(await sessionService.CloseForTokenAsync(authorization[7..].Trim()));
        }
    }
}
