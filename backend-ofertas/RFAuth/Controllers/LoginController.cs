using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class LoginController(ILoginService loginService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LoginRequest request)
        {
            var response = await loginService.LoginAsync(request);
            await loginService.AddAttributesAsync(response);
            return Ok(response);
        }
    }
}
