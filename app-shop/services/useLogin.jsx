import AsyncStorage from '@react-native-async-storage/async-storage';
import { Api } from './api';

export default function useLogin() {
  const [onLoginSuccess, setOnLoginSuccess] = useState(null);
  const [onLoginError, setOnLoginError] = useState(null);
  const [onLogout, setOnLogout] = useState(null);

  async function autoLogin() {
    const autoLoginToken = await AsyncStorage.getItem('autoLoginToken');
    if (!autoLoginToken) {
      return;
    }

    return _login('/v1/auto-login', {autoLoginToken});
  }

  async function login(body) {
    return _login('/v1/login', body);
  }

  async function _login(url, body) {
    const deviceToken = await AsyncStorage.getItem('deviceToken');
    if (deviceToken) {
      body.deviceToken = deviceToken;
    }

    try {
      const data = await Api.postJson(url, { body });
      if (data.deviceToken) {
        AsyncStorage.setItem('deviceToken', data.deviceToken);
      }
      
      if (data.autoLoginToken) {
        AsyncStorage.setItem('autoLoginToken', data.autoLoginToken);
      }
      
      if (data?.authorizationToken) {
        Api.headers.Authorization = 'Bearer ' + data.authorizationToken;
        if (onLoginSuccess) {
          onLoginSuccess(data);
        }
      }
    } catch(err) {
      if (onLoginError) {
        onLoginError(err);
      } else {
        console.error(err);
      }
    }
  }

  async function logout() {
    Api.postJson('/v1/logout');

    delete Api.headers.Authorization;
    AsyncStorage.removeItem('autoLoginToken');
    if (onLogout) {
      onLogout();
    }
  }

  return {
    onLoginSuccess, setOnLoginSuccess,
    onLoginError, setOnLoginError,
    onLogout, setOnLogout,
    autoLogin,
    login,
    logout,
  }
}