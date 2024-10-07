using AutoMapper;
using RFAuth.Entities;
using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RFService.DataLib;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("Recuperando ususarios");

            var res = (await userService.GetList()).Select(mapper.Map<User,UserResponse>);
            return Ok(new DataRowsResult(res));
        }
    }
}
