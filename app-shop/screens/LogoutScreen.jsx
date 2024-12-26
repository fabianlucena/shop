import { useReducer } from 'react';
import { View } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import { logout } from '../libs/login';
import Message from '../components/Message';

export default function LogoutScreen({ navigation }) {
  const [, forceUpdate] = useReducer(o => !o);

  async function logoutHandler() {
    await logout();
    forceUpdate();
  }

  return (
    <Background>
      <View style={styles.container}>
        <Message>Su sesión se cerrará. Para volver a utilizar la áplicación deberá iniciar sesión nuevamente.</Message>
        <View style={styles.sameLine}>
          <Button onPress={() => navigation.goBack()} >Volver</Button>
          <Button onPress={logoutHandler} >Salir</Button>
        </View>
      </View>
    </Background>
  );
}