using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using RFService.Authorization;
using RFService.Data;
using RFService.Libs;
using RFService.Repo;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/business")]
    public class BusinessController(
        ILogger<BusinessController> logger,
        IBusinessService businessService,
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

            if (uuid != null)
                await businessService.CheckForUuidAndCurrentUserAsync(uuid.Value);

            var ownerId = (HttpContext.Items["UserId"] as Int64?)
                ?? throw new NoAuthorizationHeaderException();

            var options = GetOptions.CreateFromQuery(HttpContext);
            options.AddFilter("OwnerId", ownerId);
            if (uuid != null)
                options.Filters["Uuid"] = uuid;

            var businessList = await businessService.GetListAsync(options);

            var response = businessList.Select(mapper.Map<Business, BusinessResponse>);

            logger.LogInformation("Business getted");

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("business.edit")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary data)
        {
            logger.LogInformation("Updating bussines");

            await businessService.CheckForUuidAndCurrentUserAsync(uuid);

            data = data.GetPascalized();

            var result = await businessService.UpdateForUuidAsync(uuid, data);

            if (result <= 0)
                return BadRequest();

            logger.LogInformation("Bussines updated");

            return Ok();
        }

        [HttpDelete("{uuid}")]
        [Permission("business.delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Deleting business");

            await businessService.CheckForUuidAndCurrentUserAsync(uuid);

            var result = await businessService.DeleteForUuidAsync(uuid);

            if (result <= 0)
                return BadRequest();

            logger.LogInformation("Business deleted");

            return Ok();
        }

        [HttpPost("restore/{uuid}")]
        [Permission("business.restore")]
        public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Restoring business");

            await businessService.CheckForUuidAndCurrentUserAsync(uuid, new GetOptions { Options = { { "IncludeDeleted", true } } });

            var result = await businessService.RestoreForUuidAsync(uuid);

            if (result <= 0)
                return BadRequest();

            logger.LogInformation("Business restored");

            return Ok();
        }
    }
}
