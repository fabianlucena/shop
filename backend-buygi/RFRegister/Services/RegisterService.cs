using AutoMapper;
using RFAuth.Entities;
using RFAuth.IServices;
using RFUserEmail.Entities;
using RFUserEmail.IServices;
using RFRegister.DTO;
using RFRegister.IServices;

namespace RFRegister.Services
{
    public class RegisterService(IUserService userService, IUserEmailService userEmailService, IMapper mapper) : IRegisterService
    {
        public async Task RegisterAsync(RegisterData registerData)
        {
            if (string.IsNullOrWhiteSpace(registerData.Username))
            {
                throw new ArgumentNullException(nameof(registerData.Username));
            }

            if (string.IsNullOrWhiteSpace(registerData.Password))
            {
                throw new ArgumentNullException(nameof(registerData.Password));
            }

            if (string.IsNullOrWhiteSpace(registerData.FullName))
            {
                throw new ArgumentNullException(nameof(registerData.FullName));
            }

            if (string.IsNullOrWhiteSpace(registerData.EMail))
            {
                throw new ArgumentNullException(nameof(registerData.EMail));
            }

            var user = await userService.CreateAsync(mapper.Map<RegisterData,User>(registerData));
            await userEmailService.CreateAsync(new UserEmail {
                UserId = user.Id,
                Email = registerData.EMail
            });
        }
    }
}
