import { useState } from 'react';
import { View } from 'react-native';
import TextField from '../components/TextField';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import login from '../libs/login';

export default function LoginScreen() {
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('1234');

  function loginHandler() {
    login({ username, password });
  }

  return (
    <Background>
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
        <Button onPress={loginHandler} >Ingresar</Button>
      </View>
    </Background>
  );
}