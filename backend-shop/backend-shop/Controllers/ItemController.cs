﻿using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.Exceptions;
using backend_shop.IServices;
using backend_shop.Service;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;
using RFService.Data;
using RFService.Repo;

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
        [Permission("store.add")]
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
        [Permission("category.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting items");

            var options = GetOptions.CreateFromQuery(HttpContext);
            if (uuid != null)
                options.Filters["Uuid"] = uuid;

            var itemsList = await itemService.GetListAsync(options);

            var response = itemsList.Select(mapper.Map<Item, ItemResponse>);

            logger.LogInformation("Items getted");

            return Ok(new DataRowsResult(response));
        }
    }
}
