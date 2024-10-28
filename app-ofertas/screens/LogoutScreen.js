import { useReducer } from 'react';
import { Text, View } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import { logout } from '../libs/login';
import FancyText from '../components/FancyText';

export default function LogoutScreen({ navigation }) {
  const [, forceUpdate] = useReducer(o => !o);

  async function logoutHandler() {
    await logout();
    forceUpdate();
  }

  return (
    <Background>
      <View style={styles.container}>
        <FancyText>Su sesión se cerrará. Para volver a utilizar la áplicación deberá iniciar sesión nuevamente.</FancyText>
        <View style={styles.sameLine}>
          <Button onPress={() => navigation.goBack()} >Volver</Button>
          <Button onPress={logoutHandler} >Salir</Button>
        </View>
      </View>
    </Background>
  );
}