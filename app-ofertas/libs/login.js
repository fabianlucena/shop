import AsyncStorage from '@react-native-async-storage/async-storage';
import { Api } from './api';

let onLoginSuccess;
export function setOnLoginSuccess(newOnLoginSuccess) {
  onLoginSuccess = newOnLoginSuccess;
}

let onLogout;
export function setOnLogout(newOnLogout) {
  onLogout = newOnLogout;
}

let onLoginError;
export function setOnLoginError(newOnLoginError) {
  onLoginError = newOnLoginError;
}

export default async function login(body) {
  if (!body) {
    const autoLoginToken = await AsyncStorage.getItem('autoLoginToken');
    if (!autoLoginToken) {
      return;
    }

    body = {autoLoginToken};
  }

  const deviceToken = await AsyncStorage.getItem('deviceToken');
  if (deviceToken) {
    body.deviceToken = deviceToken;
  }

  try {
    const data = await Api.postJson('/login', { body });
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

export async function logout() {
  Api.postJson('/logout');

  delete Api.headers.Authorization;
  AsyncStorage.removeItem('autoLoginToken');
  if (onLogout) {
    onLogout();
  }
}