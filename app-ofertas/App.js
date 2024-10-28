import '@expo/metro-runtime';
import {NavigationContainer, createNavigationContainerRef } from '@react-navigation/native';
import {createNativeStackNavigator} from '@react-navigation/native-stack';
import LoginScreen from './screens/LoginScreen';
import RegisterScreen from './screens/RegisterScreen';
import HomeScreen from './screens/HomeScreen';
import LogoutScreen from './screens/LogoutScreen';
import ChangePasswordScreen from './screens/ChangePassword';
import { Api } from './libs/api';
import { useState, useEffect } from 'react';
import { View, Text } from 'react-native';
import { autoLogin, setOnLoginSuccess, setOnLoginError, setOnLogout } from './libs/login';
import Background from './components/Background';
import FancyText from './components/FancyText';
import styles from './libs/styles';

const Stack = createNativeStackNavigator();
const navigationRef = createNavigationContainerRef();

export default function App() {
  const [initiated, setInitiated] = useState();
  const [loggued, setLogged] = useState(!!Api.headers.Authorization);

  useEffect(() => {
    (async () => {
      setOnLoginSuccess(() => setLogged(true));
      setOnLogout(() => setLogged(false));
      setOnLoginError(err => console.error(err));
      await autoLogin();
      setInitiated(true);
    })();
  }, []);

  if (!initiated) {
    return (
      <Background>
        <View style={styles.container}>
          <FancyText>
            Iniciando
          </FancyText>
        </View>
      </Background>
    );
  }

  if (!loggued) {
    return (
      <NavigationContainer>
        <Stack.Navigator >
          <Stack.Screen name="Login"    component={LoginScreen}    options={{ title: 'Ingresar' }}/>
          <Stack.Screen name="Register" component={RegisterScreen} options={{ title: 'Registrarse' }}/>
        </Stack.Navigator>
      </NavigationContainer>
    );
  }

  return (
    <NavigationContainer ref={navigationRef}>
      <Stack.Navigator >
        <Stack.Screen name="Home"           component={HomeScreen}           options={{ title: 'Principal' }}/>
        <Stack.Screen name="ChangePassword" component={ChangePasswordScreen} options={{ title: 'Cambiar contraseña' }}/>
        <Stack.Screen name="Logout"         component={LogoutScreen}         options={{ title: 'Salir' }}/>
      </Stack.Navigator>
    </NavigationContainer>
  );
}
