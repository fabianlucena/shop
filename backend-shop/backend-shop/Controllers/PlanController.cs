using backend_shop.IServices;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/plan")]
    public class PlanController(
        ILogger<PlanController> logger,
        IServiceProvider serviceProvider
    )
        : ControllerBase
    {
        [HttpGet]
        [Permission("plan.get")]
        public async Task<IActionResult> GetAsync()
        {
            logger.LogInformation("Getting plan");

            var userPlanService = serviceProvider.GetRequiredService<IUserPlanService>();
            var planService = serviceProvider.GetRequiredService<IPlanService>();

            var plan = await userPlanService.GetSinglePlanForCurrentUserAsync();
            var limits = await planService.GetLimitsForPlanAsync(plan);
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
