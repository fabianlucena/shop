import '@expo/metro-runtime';
import {NavigationContainer, createNavigationContainerRef } from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import LoginScreen from './screens/LoginScreen';
import HomeScreen from './screens/HomeScreen';
import { Api } from './libs/api';
import { useState, useEffect } from 'react';
import login, { setOnLoginSuccess, setOnLoginError } from './libs/login';

const Stack = createNativeStackNavigator();
const navigationRef = createNavigationContainerRef();

export default function App() {
  const [loggued, setLogged] = useState(!!Api.headers.Authorization);

  useEffect(() => {
    setOnLoginSuccess(() => setLogged(true));
    setOnLoginError(err => console.error(err));
    login();
  }, []);

  if (!loggued) {
    return (<LoginScreen />);
  }

  return (
    <NavigationContainer ref={navigationRef}>
      <Stack.Navigator >
        <Stack.Screen name="Home"  component={HomeScreen}  options={{ title: 'Principal' }}/>
      </Stack.Navigator>
    </NavigationContainer>
  );
}
