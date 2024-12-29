import { useReducer } from 'react';
import { View } from 'react-native';
import Button from '../components/Button';
import Screen from '../components/Screen';
import styles from '../libs/styles';
import useLogin from '../services/useLogin';
import Message from '../components/Message';

export default function LogoutScreen({ navigation }) {
  const [, forceUpdate] = useReducer(o => !o);
  const { logout } = useLogin();

  async function logoutHandler() {
    logout()
      .finally(() => forceUpdate());
  }

  return (
    <Screen
      header="Salir"
    >
      <Message>Su sesión se cerrará. Para volver a utilizar la áplicación deberá iniciar sesión nuevamente.</Message>
      <View style={styles.sameLine}>
        <Button onPress={() => navigation.goBack()} >Volver</Button>
        <Button onPress={logoutHandler} >Salir</Button>
      </View>
    </Screen>
  );
}