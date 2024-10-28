using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class AutoLoginController(ILoginService loginService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] AutoLoginRequest request)
        {
            var response = await loginService.AutoLoginAsync(request);
            await loginService.AddAttributesAsync(response);
            return Ok(response);
        }
    }
}
