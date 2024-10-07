using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController(ILoginService loginService) : ControllerBase
    {
        private readonly ILoginService _loginService = loginService;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginData loginData)
        {
            return Ok(await _loginService.Login(loginData));
        }
    }
}
