import { useReducer } from 'react';
import { View } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import useLogin from '../services/useLogin';
import Message from '../components/Message';
import { useSession } from '../contexts/Session';

export default function LogoutScreen({ navigation }) {
  const [, forceUpdate] = useReducer(o => !o);
  const { logout } = useLogin();
  const { setIsLoggedIn } = useSession();

  async function logoutHandler() {
    logout()
      .catch(err => console.error(err))
      .finally(() => {
        setIsLoggedIn(false);
        forceUpdate();
      });
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