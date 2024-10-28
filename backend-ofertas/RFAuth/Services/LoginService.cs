using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.Exceptions;
using RFAuth.IServices;
using RFService.ServicesLib;

namespace RFAuth.Services
{
    public class LoginService(
        IUserService userService,
        IPasswordService passwordService,
        IDeviceService deviceService,
        ISessionService sessionService
    ) : ServiceAttributes, ILoginService
    {
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentNullException(nameof(LoginRequest.Username));

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentNullException(nameof(LoginRequest.Password));

            var user = await userService.GetSingleForUsernameAsync(request.Username);
            var password = await passwordService.GetSingleForUserAsync(user);
            var check = passwordService.Verify(request.Password, password);
            if (!check)
                throw new BadPasswordException();

            var device = await deviceService.GetSingleForTokenOrCreateAsync(request.DeviceToken);
            var session = await sessionService.CreateForUserAndDeviceAsync(user, device);
            
            return new LoginResponse
            {
                AuthorizationToken = session.Token,
                AutoLoginToken = session.AutoLoginToken,
                DeviceToken = device.Token,
            };
        }

        public async Task<LoginResponse> AutoLoginAsync(AutoLoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AutoLoginToken))
                throw new ArgumentNullException(nameof(AutoLoginRequest.AutoLoginToken));

            if (string.IsNullOrWhiteSpace(request.DeviceToken))
                throw new ArgumentNullException(nameof(AutoLoginRequest.DeviceToken));

            var device = await deviceService.GetSingleOrDefaultForTokenAsync(request.DeviceToken)
                ?? throw new UnknownDeviceException();

            var session = await sessionService.CreateForAutoLoginTokenAndDeviceAsync(request.AutoLoginToken, device);

            return new LoginResponse
            {
                AuthorizationToken = session.Token,
                AutoLoginToken = session.AutoLoginToken,
                DeviceToken = device.Token,
            };
        }
    }
}
