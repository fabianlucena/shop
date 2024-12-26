import { useEffect } from 'react';
import { NavigationContainer, createNavigationContainerRef } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { View, ActivityIndicator } from 'react-native';

import LoginScreen from './screens/LoginScreen';
import RegisterScreen from './screens/RegisterScreen';
import Background from './components/Background';
import FancyText from './components/FancyText';
import styles from './libs/styles';
import { useSession } from './contexts/Session';
import useLogin from './services/useLogin';
import MainScreen from './screens/MainScreen';

const Stack = createNativeStackNavigator();

export default function Shop() {
  const { isInitiated, setIsInitiated, isLogguedIn } = useSession();
  const { autoLogin } = useLogin();

  useEffect(() => {
      autoLogin()
        .finally(() => setIsInitiated(true));
    }, []);

  if (!isInitiated) {
    return (
      <Background>
        <View style={styles.container}>
          <FancyText>
            Iniciando
          </FancyText>
          <ActivityIndicator size="large" color="#000000" style={{...styles.loader }} />
        </View>
      </Background>
    );
  }

  if (!isLogguedIn) {
    return (
      <NavigationContainer>
        <Stack.Navigator >
          <Stack.Screen name="Login"    component={LoginScreen}    options={{ title: 'Ingresar' }}/>
          <Stack.Screen name="Register" component={RegisterScreen} options={{ title: 'Registrarse' }}/>
        </Stack.Navigator>
      </NavigationContainer>
    );
  }

  return <MainScreen />;
}
