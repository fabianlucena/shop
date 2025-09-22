import { useEffect } from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

import { useSession } from '../components/Session';

import LoadingScreen from './LoadingScreen';
import LoginScreen from './LoginScreen';
import RegisterScreen from './RegisterScreen';
import Content from './ContentScreen';

import useLogin from '../services/useLogin';

const Stack = createNativeStackNavigator();

export default function AppScreen() {
  const { isInitiated, setIsInitiated, isLogguedIn } = useSession();
  const { autoLogin } = useLogin();

  useEffect(() => {
    autoLogin()
      .finally(() => setIsInitiated(true));
  }, []);

  if (!isInitiated)
    return <LoadingScreen message="Iniciando" />

  if (!isLogguedIn) {
    return (
      <NavigationContainer>
        <Stack.Navigator >
          <Stack.Screen name="Login"    component={LoginScreen}    options={{ headerShown: false, title: 'Ingresar' }}/>
          <Stack.Screen name="Register" component={RegisterScreen} options={{ headerShown: false, title: 'Registrarse' }}/>
        </Stack.Navigator>
      </NavigationContainer>
    );
  }

  return <Content />;
}
