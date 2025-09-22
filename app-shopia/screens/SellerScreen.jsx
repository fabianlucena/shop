import { useReducer } from 'react';
import { View } from 'react-native';
import Button from '../components/Button';
import Screen from '../components/Screen';
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
    <Screen>
      <View style={styles.sameLine}>
        <Button onPress={() => navigation.goBack()} >Mercado</Button>
      </View>
    </Screen>
  );
}