using AutoMapper;
using backend_buygi.Entities;
using backend_buygi.DTO;
using backend_buygi.IServices;
using Microsoft.AspNetCore.Mvc;

namespace backend_buygi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = (await _userService.GetList()).Select(_mapper.Map<User,UserResponse>);
            return Ok(new DataRowsResult(res));
        }
    }
}
