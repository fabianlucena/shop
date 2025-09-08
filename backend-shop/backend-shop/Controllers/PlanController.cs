using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/plan")]
    public class PlanController(
        ILogger<PlanController> logger,
        IUserPlanService userPlanService,
        IMapper mapper
    )
        : ControllerBase
    {
        [HttpGet]
        [Permission("plan.get")]
        public async Task<IActionResult> GetAsync()
        {
            logger.LogInformation("Getting plan");

            var plan = await userPlanService.GetSinglePlanForCurrentUserAsync();
            var response = new {
                Available = mapper.Map<Plan, PlanResponse>(plan)
            };

            logger.LogInformation("Plan retrieved");

            return Ok(response);
        }
    }
}
