import AsyncStorage from '@react-native-async-storage/async-storage';
import { Api } from '../libs/api';

export default function useLogin() {
  async function autoLogin() {
    const autoLoginToken = await AsyncStorage.getItem('autoLoginToken');
    if (!autoLoginToken) {
      return false;
    }

    await _login('/v1/auto-login', {autoLoginToken});
    return true;
  }

  async function login(body) {
    await _login('/v1/login', body);
    return true;
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
      }
    } catch(err) {
      console.error(err);
      throw err;
    }
  }

  async function logout() {
    AsyncStorage.removeItem('autoLoginToken');
    
    Api.postJson('/v1/logout');
    delete Api.headers.Authorization;
  }

  return {
    autoLogin,
    login,
    logout,
  }
}