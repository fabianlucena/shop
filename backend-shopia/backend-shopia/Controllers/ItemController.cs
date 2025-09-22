using AutoMapper;
using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.Exceptions;
using backend_shopia.IServices;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;
using RFService.Data;
using RFService.Libs;
using RFService.Repo;
using Sprache;
using System.Text.Json;

namespace backend_shopia.Controllers
{
    [ApiController]
    [Route("v1/item")]
    public class ItemController(
        ILogger<ItemController> logger,
        IItemService itemService,
        IItemFileService itemFileService,
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

            var updateImagesResult = await UpdateImages(result.Id);
            if (updateImagesResult is BadRequestObjectResult)
                return updateImagesResult;

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
                var uuidList = (await itemService.GetListUuidForCurrentUserAsync()).ToHashSet();
                var itemIdMap = itemsList.ToDictionary(i => i.Uuid, i => i.Id);

                response = await Task.WhenAll(response.Select(async item =>
                {
                    item.IsMine = uuidList.Contains(item.Uuid);
                    if (itemIdMap.TryGetValue(item.Uuid, out var itemId))
                    {
                        var files = await itemFileService.GetListForItemIdAsync(itemId);
                        item.Images = [.. files.Select(f => new ItemImageDTO {
                            Uuid = f.Uuid,
                            Uri = $"/v1/item/image/{f.Uuid}",
                        }).ToList()];
                    }

                    return item;
                }));
            }

            logger.LogInformation("Items retrieved");

            return Ok(new DataRowsResult(response));
        }

        [HttpPatch("{uuid}")]
        [Permission("item.edit")]
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

            var updateImagesResult = await UpdateImages(uuid);
            if (updateImagesResult is BadRequestObjectResult)
                return updateImagesResult;

            if (data.ContainsKey("IsEnabled"))
                _ = await itemService.UpdateInheritedForUuid(uuid);

            logger.LogInformation("Item updated");

            return Ok();
        }

        private async Task<IActionResult> UpdateImages(Guid itemUuid)
            => await UpdateImages(
                await itemService.GetSingleIdForUuidAsync(
                    itemUuid,
                    new QueryOptions
                    {
                        Switches = { { "IncludeDisabled", true } }
                    }
                )
            );

        private async Task<IActionResult> UpdateImages(Int64 itemId)
        {
            if (!Request.HasFormContentType)
                return Ok();

            if (Request.Form.Files.Count > 0)
            {
                var files = new FilesCollectionDTO(Request.Form.Files);
                foreach (var file in Request.Form.Files)
                {
                    if (file.Length == 0)
                        continue;

                    if (!file.ContentType.StartsWith("image/"))
                        return BadRequest("Only image files are allowed.");
                }

                var result = await itemFileService.AddForItemIdAsync(itemId, files);
                if (!result.Any())
                    return BadRequest("Error uploading image.");
            }

            var deletedImagesStrings = Request.Form["deletedImages"];
            if (deletedImagesStrings.Count == 0)
                return Ok();

            if (deletedImagesStrings.Count > 1)
                return BadRequest("Error multiple deleted images objects.");

            var deletedImages = JsonSerializer.Deserialize<List<Guid>>(deletedImagesStrings[0]!);
            foreach (var uuid in deletedImages!)
            {
                var result = await itemFileService.DeleteForUuidAsync(uuid);
                if (result <= 0)
                    return BadRequest();
            }

            return Ok();
        }

        [HttpGet("image/{uuid}")]
        public async Task<IActionResult> GetImageAsync([FromRoute] Guid uuid)
        {
            logger.LogInformation("Getting item image for UUID: {Uuid}", uuid);

            var file = await itemFileService.GetSingleOrDefaultForUuidAsync(uuid)
                ?? throw new ItemImageNotFoundException();

            logger.LogInformation("Item image retrieved for UUID: {Uuid}", uuid);

            return File(file.Content, file.ContentType);
        }
    }
}
