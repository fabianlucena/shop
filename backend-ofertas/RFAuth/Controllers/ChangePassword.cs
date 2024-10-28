using RFAuth.DTO;
using RFAuth.IServices;
using Microsoft.AspNetCore.Mvc;
using RFAuth.Exceptions;
using RFAuth.Services;

namespace RFAuth.Controllers
{
    [ApiController]
    [Route("v1/change-password")]
    public class ChangePasswordController(IPasswordService passwordService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ChangePasswordRequest request)
        {

            Int64 userId = Objeter el userId desde el contexto de autorización;
            var password = await passwordService.GetSingleForUserIdAsync(userId);
            var check = passwordService.Verify(request.CurrentPassword, password);
            if (!check)
                throw new BadPasswordException();

            await passwordService.UpdateForIdAsync(
                new { password = passwordService.Hash(request.NewPassword) },
                password.Id
            );

            return Ok();
        }
    }
}
