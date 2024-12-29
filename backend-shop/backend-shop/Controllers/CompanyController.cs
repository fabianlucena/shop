using AutoMapper;
using backend_shop.DTO;
using backend_shop.Entities;
using backend_shop.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.Exceptions;
using RFAuth.Services;
using RFService.Authorization;
using RFService.Data;
using RFService.Repo;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/company")]
    public class CompanyController(
        ILogger<CompanyController> logger,
        ICompanyService companyService,
        IMapper mapper
    )
        : ControllerBase
    {
        [HttpPost]
        [Permission("company.add")]
        public async Task<IActionResult> PostAsync([FromBody] CompanyAddRequest data)
        {
            logger.LogInformation("Creating company");

            var company = mapper.Map<CompanyAddRequest, Company>(data);
            company.OwnerId = (HttpContext?.Items["UserId"] as Int64?)
                ?? throw new NoAuthorizationHeaderException();

            var result = await companyService.CreateAsync(company);

            if (result == null)
                return BadRequest();

            logger.LogInformation("Company created");

            return Ok();
        }

        [HttpGet("{uuid?}")]
        [Permission("company.get")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid? uuid)
        {
            logger.LogInformation("Getting company");

            var options = GetOptions.CreateFromQuery(HttpContext);
            if (uuid != null)
                options.Filters["Uuid"] = uuid;

            var companyList = await companyService.GetListAsync(options);

            var response = companyList.Select(mapper.Map<Company, CompanyResponse>);

            logger.LogInformation("Company getted");

            return Ok(new DataRowsResult(response));
        }
    }
}
