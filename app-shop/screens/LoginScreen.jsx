import { useState } from 'react';
import { View } from 'react-native';
import TextField from '../components/TextField';
import Button from '../components/Button';
import Background from '../components/Background';
import Busy from '../components/Busy';
import styles from '../libs/styles';
import { login } from '../services/useLogin';

export default function LoginScreen({ navigation }) {
  const [loading, setLoading] = useState(false);
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('1234');

  async function loginHandler() {
    setLoading(true);
    await login({ username, password });
    setLoading(false);
  }

  return (
    <Background>
      <Busy busy={loading}>
        <View style={styles.container}>
          <TextField
            value={username}
            onChangeValue={setUsername}
          >
            Nombre de Usuario
          </TextField>
          <TextField
            value={password}
            onChangeValue={setPassword}
            secureTextEntry={true}
          >
            Contraseña
          </TextField>
          <View style={styles.sameLine}>
            <Button onPress={() => navigation.navigate('Register')} disabled={loading} >Registrarse</Button>
            <Button onPress={loginHandler} disabled={loading} >Ingresar</Button>
          </View>
        </View>
      </Busy>
    </Background>
  );
}