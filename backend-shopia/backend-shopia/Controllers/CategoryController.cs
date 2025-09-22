using AutoMapper;
using backend_shopia.DTO;
using backend_shopia.Entities;
using backend_shopia.IServices;
using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;
using RFService.Data;
using RFService.Repo;

namespace backend_shopia.Controllers
{
    [ApiController]
    [Route("v1/category")]
    public class CategoryController(
        ILogger<CategoryController> logger,
        ICategoryService categoryService,
        IMapper mapper
    )
        : ControllerBase
    {
        [HttpGet("{uuid?}")]
        [Permission("category.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting categories");

            var options = QueryOptions.CreateFromQuery(HttpContext);
            if (uuid != null)
                options.AddFilter("Uuid", uuid);

            var categoriesList = await categoryService.GetListAsync(options);

            var response = categoriesList.Select(mapper.Map<Category, CategoryResponse>);

            logger.LogInformation("Categories retrieved");

            return Ok(new DataRowsResult(response));
        }
    }
}
