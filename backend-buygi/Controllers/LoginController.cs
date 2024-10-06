using AutoMapper;
using backend_buygi.DTO;
using backend_buygi.IServices;
using Microsoft.AspNetCore.Mvc;

namespace backend_buygi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginData loginData)
        {
            return Ok(await _loginService.login(loginData));
        }
    }
}
