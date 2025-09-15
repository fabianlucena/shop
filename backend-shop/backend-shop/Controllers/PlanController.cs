using backend_shop.IServices;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/plan")]
    public class PlanController(
        ILogger<PlanController> logger,
        IUserPlanService userPlanService
    )
        : ControllerBase
    {
        [HttpGet]
        [Permission("plan.get")]
        public async Task<IActionResult> GetAsync()
        {
            logger.LogInformation("Getting plan");

            var plan = await userPlanService.GetSinglePlanForCurrentUserAsync();
            var limits = await userPlanService.GetLimitsForCurrentUserAsync();
            var used = await userPlanService.GetUsedPlanForCurrentUserAsync();
            var response = new {
                plan.Uuid,
                plan.Name,
                plan.Description,
                Limits = limits.ToDictionaryLCFirst(),
                Used = used,
            };

            logger.LogInformation("Plan retrieved");

            return Ok(response);
        }
    }
}
