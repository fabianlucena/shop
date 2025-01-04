using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;
using RFService.Data;
using RFService.Libs;
using RFService.Repo;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/store")]
    public class StoreController(
        ILogger<StoreController> logger,
        IStoreService storeService,
        IMapper mapper,
        ICommerceService commerceService,
        IItemService itemService
    )
        : ControllerBase
    {
        [HttpPost]
        [Permission("store.add")]
        public async Task<IActionResult> PostAsync([FromBody] StoreAddRequest data)
        {
            logger.LogInformation("Creating store");

            if (data.CommerceUuid == default)
                throw new NoCommerceException();

            var store = mapper.Map<StoreAddRequest, Store>(data);

            var commercesIdList = await commerceService.GetListIdForCurrentUserAsync();
            if (!commercesIdList.Contains(store.CommerceId))
                throw new CommerceDoesNotExistException();

            var result = await storeService.CreateAsync(store);

            if (result == null)
                return BadRequest();

            logger.LogInformation("Store created");

            return Ok();
        }

        [HttpGet("{uuid?}")]
        [Permission("store.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting stores");

            if (uuid != null)
                await storeService.CheckForUuidAndCurrentUserAsync(uuid.Value);

            var commercesIdList = await commerceService.GetListIdForCurrentUserAsync();

            var options = GetOptions.CreateFromQuery(HttpContext);
            options.AddFilter("CommerceId", commercesIdList);
            if (uuid != null)
                options.Filters["Uuid"] = uuid;

            var storeList = await storeService.GetListAsync(options);

            var response = storeList.Select(mapper.Map<Store, StoreResponse>);

            logger.LogInformation("Stores getted");

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("store.edit")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid, [FromBody] DataDictionary data)
        {
            logger.LogInformation("Updating commerce");

            await storeService.CheckForUuidAndCurrentUserAsync(uuid);

            data = data.GetPascalized();

            var result = await storeService.UpdateForUuidAsync(uuid, data);

            if (result <= 0)
                return BadRequest();

            _ = itemService.UpdateInheritedForStoreUuid(uuid);

            logger.LogInformation("Busines updated");

            return Ok();
        }

        [HttpDelete("{uuid}")]
        [Permission("store.delete")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Deleting store");

            await storeService.CheckForUuidAndCurrentUserAsync(uuid);

            var result = await storeService.DeleteForUuidAsync(uuid);

            if (result <= 0)
                return BadRequest();

            _ = itemService.UpdateInheritedForStoreUuid(uuid);

            logger.LogInformation("Store deleted");

            return Ok();
        }

        [HttpPost("restore/{uuid}")]
        [Permission("store.restore")]
        public async Task<IActionResult> RestoreAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Restoring store");

            await storeService.CheckForUuidAndCurrentUserAsync(uuid, new GetOptions { Options = { { "IncludeDeleted", true } } });

            var result = await storeService.RestoreForUuidAsync(uuid);

            if (result <= 0)
                return BadRequest();

            _ = itemService.UpdateInheritedForStoreUuid(uuid);

            logger.LogInformation("Store restored");

            return Ok();
        }
    }
}
