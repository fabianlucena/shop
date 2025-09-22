using Microsoft.AspNetCore.Mvc;
using RFService.Authorization;
using RFService.Libs;
using RFService.Repo;
using RFService.Data;
using AutoMapper;
using RFOperators;
using RFLoggerProvider.DTO;
using RFLoggerProvider.Entities;
using RFLoggerProvider.IServices;

namespace backend_shop.Controllers
{
    [ApiController]
    [Route("v1/log")]
    public class LogController(
        ILogger<LogController> logger,
        ILogService logService,
        IMapper mapper
    ) : ControllerBase
    {
        [HttpGet]
        [Permission("log.get")]
        public async Task<IActionResult> GetAsync()
        {
            logger.LogInformation("Getting log");

            var query = HttpContext.Request.Query.GetPascalized();
            var options = QueryOptions.CreateFromQuery(query);

            options.Include("Level");
            options.Include("Action"); 
            options.Include("Session", "s");
            options.Include("Project", "p");
            options.Include(
                "User",
                "u",
                type: JoinType.Left,
                on: Op.Eq(Op.Column("s.UserId"), Op.Column("u.Id"))
            );

            var result = (await logService.GetListAsync(options))
                .Select(mapper.Map<Log, LogResponse>);

            return Ok(new DataRowsResult(result));
        }
    }
}
