using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using RFService.Authorization;
using RFService.Data;
using RFService.Repo;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/business")]
    public class BusinessController(
        ILogger<BusinessController> logger,
        IBusinessService businessService,
        IPlanService planService,
        IMapper mapper
    )
        : ControllerBase
    {
        [HttpPost]
        [Permission("business.add")]
        public async Task<IActionResult> PostAsync([FromBody] BusinessAddRequest data)
        {
            logger.LogInformation("Creating business");

            var business = mapper.Map<BusinessAddRequest, Business>(data);
            business.OwnerId = (HttpContext?.Items["UserId"] as Int64?)
                ?? throw new NoAuthorizationHeaderException();
            business.PlanId = await planService.GetSingleIdForNameAsync("Base");

            var result = await businessService.CreateAsync(business);

            if (result == null)
                return BadRequest();

            logger.LogInformation("Business created");

            return Ok();
        }

        [HttpGet("{uuid?}")]
        [Permission("business.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting business");

            var options = GetOptions.CreateFromQuery(HttpContext);
            if (uuid != null)
                options.Filters["Uuid"] = uuid;

            var businessList = await businessService.GetListAsync(options);

            var response = businessList.Select(mapper.Map<Business, BusinessResponse>);

            logger.LogInformation("Business getted");

            return Ok(new DataRowsResult(response));
        }
    }
}
