using backend_buygi.DTO;
using backend_buygi.Exceptions;
using backend_buygi.IServices;

namespace backend_buygi.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IDeviceService _deviceService;
        private readonly ISessionService _sessionService;

        public LoginService(IUserService userService, IPasswordService passwordService, IDeviceService deviceService, ISessionService sessionService)
        {
            _userService = userService;
            _deviceService = deviceService;
            _passwordService = passwordService;
            _sessionService = sessionService;
        }

        public async Task<AuthorizationData> login(LoginData loginData)
        {
            if (string.IsNullOrWhiteSpace(loginData.Username)) {
                throw new ArgumentNullException(nameof(loginData.Username));
            }
            var user = await _userService.GetSingleForUsername(loginData.Username);

            if (string.IsNullOrWhiteSpace(loginData.Password)) {
                throw new ArgumentNullException(nameof(loginData.Password));
            }

            var password = await _passwordService.GetSingleForUser(user);
            var check = _passwordService.Verify(loginData.Password, password);
            if (!check) {
                throw new BadPasswordException();
            }

            var device = await _deviceService.GetSingleForTokenOrCreate(loginData.DeviceToken);
            var session = await _sessionService.CreateForUserAndDevice(user, device);

            return new AuthorizationData
            {
                AuthorizationToken = session.Token,
                AutoLoginToken = session.AutoLoginToken,
                DeviceToken = device.Token,
            }; ;
        }
    }
}
