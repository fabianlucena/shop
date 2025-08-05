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
using System.Text.Json;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/item")]
    public class ItemController(
        ILogger<ItemController> logger,
        IItemService itemService,
        IMapper mapper
    )
        : ControllerBase
    {
        [HttpPost]
        [Permission("item.add")]
        public async Task<IActionResult> PostAsync([FromBody] ItemAddRequest data)
        {
            logger.LogInformation("Creating item");

            if (data.CategoryUuid == default)
                throw new NoCategoryException();

            if (data.StoreUuid == default)
                throw new NoStoreException();

            var item = mapper.Map<ItemAddRequest, Item>(data);

            var result = await itemService.CreateAsync(item);

            if (result == null)
                return BadRequest();

            logger.LogInformation("Item created");

            return Ok();
        }

        [HttpGet("{uuid?}")]
        [Permission("item.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting items");

            var options = QueryOptions.CreateFromQuery(HttpContext);
            if (uuid != null)
                options.AddFilter("Uuid", uuid);

            options
                .AddFilter("InheritedIsEnabled", true)
                .Include("Category")
                .Include("Store");
            var itemsList = await itemService.GetListAsync(options);

            var response = itemsList.Select(mapper.Map<Item, ItemResponse>);

            if (response.Any())
            {
                var uuidList = (await itemService.GetListUuidForCurrentUserAsync()).ToList();
                if (uuidList.Count > 0)
                {
                    foreach (var item in response)
                        item.IsMine = uuidList.Contains(item.Uuid);
                }
            }

            logger.LogInformation("Items getted");

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("item.get")]
        public async Task<IActionResult> PatchAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Updating item");

            DataDictionary data;
            if (Request.HasFormContentType)
            {
                data = [];
                var formData = Request.Form;
                foreach (var key in formData.Keys)
                    data[key] = formData[key];
            }
            else
            {
                using var reader = new StreamReader(Request.Body);
                string bodyContent = await reader.ReadToEndAsync();
                data = JsonSerializer.Deserialize<DataDictionary>(bodyContent)!
                    .GetPascalized();
            }

            var result = await itemService.UpdateForUuidAsync(data, uuid);

            if (result <= 0)
                return BadRequest();

            if (data.ContainsKey("IsEnabled"))
                _ = await itemService.UpdateInheritedForUuid(uuid);

            logger.LogInformation("Commerce updated");

            return Ok();
        }
    }
}
