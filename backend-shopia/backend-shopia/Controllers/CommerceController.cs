using AutoMapper;
using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using RFService.Authorization;
using RFService.Data;
using RFService.Libs;
using RFService.Repo;

namespace backend_shopia.Controllers
{
    [ApiController]
    [Route("v1/commerce")]
    public class CommerceController(
        ILogger<CommerceController> logger,
        ICommerceService commerceService,
        IMapper mapper,
        IItemService itemService
    )
        : ControllerBase
    {
        [HttpPost]
        [Permission("commerce.add")]
        public async Task<IActionResult> PostAsync([FromBody] CommerceAddRequest data)
        {
            logger.LogInformation("Creating commerce");

            var commerce = mapper.Map<CommerceAddRequest, Commerce>(data);
            commerce.OwnerId = (HttpContext?.Items["UserId"] as Int64?)
                ?? throw new NoAuthorizationHeaderException();

            var result = await commerceService.CreateAsync(commerce);

            if (result == null)
                return BadRequest();

            logger.LogInformation("Commerce created");

            return Ok();
        }

        [HttpGet("{uuid?}")]
        [Permission("commerce.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting commerces");

            if (uuid != null)
                await commerceService.CheckForUuidAndCurrentUserAsync(uuid.Value);

            var ownerId = (HttpContext.Items["UserId"] as Int64?)
                ?? throw new NoAuthorizationHeaderException();

            var options = QueryOptions.CreateFromQuery(HttpContext);
            options.AddFilter("OwnerId", ownerId);
            if (uuid != null)
                options.AddFilter("Uuid", uuid);

            var commerceList = await commerceService.GetListAsync(options);

            var response = commerceList.Select(mapper.Map<Commerce, CommerceResponse>);

            logger.LogInformation("Commerces retrieved");

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("commerce.edit")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary data)
        {
            logger.LogInformation("Updating commerce");

            await commerceService.CheckForUuidAndCurrentUserAsync(uuid);

            data = data.GetPascalized();

            var result = await commerceService.UpdateForUuidAsync(data, uuid);

            if (result <= 0)
                return BadRequest();

            _ = await itemService.UpdateInheritedForCommerceUuid(uuid);

            logger.LogInformation("Commerce updated");

            return Ok();
        }

        [HttpDelete("{uuid}")]
        [Permission("commerce.delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Deleting commerce");

            await commerceService.CheckForUuidAndCurrentUserAsync(uuid);

            var result = await commerceService.DeleteForUuidAsync(uuid);

            if (result <= 0)
                return BadRequest();
            
            _ = await itemService.UpdateInheritedForCommerceUuid(uuid);

            logger.LogInformation("Commerce deleted");

            return Ok();
        }

        [HttpPost("restore/{uuid}")]
        [Permission("commerce.restore")]
        public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Restoring commerce");

            await commerceService.CheckForUuidAndCurrentUserAsync(
                uuid,
                new QueryOptions { Switches = { { "IncludeDeleted", true } } }
            );

            var result = await commerceService.RestoreForUuidAsync(uuid);

            if (result <= 0)
                return BadRequest();

            _ = await itemService.UpdateInheritedForCommerceUuid(uuid);

            logger.LogInformation("Commerce restored");

            return Ok();
        }
    }
}
