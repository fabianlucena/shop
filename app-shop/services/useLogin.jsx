import AsyncStorage from '@react-native-async-storage/async-storage';
import { Api } from '../libs/api';
import { useSession } from '../contexts/Session';

export default function useLogin() {
  const { setIsLoggedIn, setPermissions } = useSession();

  async function autoLogin() {
    const autoLoginToken = await AsyncStorage.getItem('autoLoginToken');
    if (!autoLoginToken) {
      return false;
    }

    await _login('/v1/auto-login', {autoLoginToken});
    return true;
  }

  async function login(body) {
    return await _login('/v1/login', body);
  }

  async function _login(url, body) {
    const deviceToken = await AsyncStorage.getItem('deviceToken');
    if (deviceToken) {
      body.deviceToken = deviceToken;
    }

    try {
      const data = await Api.postJson(url, { body });
      if (!data?.authorizationToken) {
        setIsLoggedIn(false);
        return;
      }

      Api.headers.Authorization = 'Bearer ' + data.authorizationToken;
      setIsLoggedIn(true);

      if (data.deviceToken) {
        AsyncStorage.setItem('deviceToken', data.deviceToken);
      }
      
      if (data.autoLoginToken) {
        AsyncStorage.setItem('autoLoginToken', data.autoLoginToken);
      }

      if (data.attributes?.permissions) {
        setPermissions(data.attributes?.permissions);
      }

      return true;
    } catch(err) {
      console.error(err);
      throw err;
    }
  }

  async function logout() {
    AsyncStorage.removeItem('autoLoginToken');
    setIsLoggedIn(false);
    
    try {
      Api.postJson('/v1/logout');
    } catch(err) {
      console.error(err);
    }
    delete Api.headers.Authorization;
  }

  return {
    autoLogin,
    login,
    logout,
  }
}