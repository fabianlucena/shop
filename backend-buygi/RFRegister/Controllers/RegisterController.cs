using Microsoft.AspNetCore.Mvc;
using RFRegister.DTO;
using RFRegister.IServices;

namespace RFRegister.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController(IRegisterService registerService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RegisterData registerData)
        {
            await registerService.RegisterAsync(registerData);
            return Ok();
        }
    }
}
