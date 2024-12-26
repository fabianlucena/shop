import { useReducer } from 'react';
import { View } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import useLogin from '../services/useLogin';

export default function LogoutScreen({ navigation }) {
  const [, forceUpdate] = useReducer(o => !o);
  const { logout } = useLogin();

  async function logoutHandler() {
    logout()
      .finally(() => forceUpdate());
  }

  return (
    <Background>
      <View style={styles.container}>
        <View style={styles.sameLine}>
          <Button onPress={() => navigation.goBack()} >Mercado</Button>
        </View>
      </View>
    </Background>
  );
}