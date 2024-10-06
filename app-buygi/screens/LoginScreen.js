import { useState } from 'react';
import { View } from 'react-native';
import TextField from '../components/TextField';
import Button from '../components/Button';
import Background from '../components/Background';
import { Api } from '../libs/api';
import styles from '../libs/styles';

export default function LoginScreen({setLogged}) {
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('1234');

  function login() {
    const body = { username, password };
    Api.postJson('/login', { body })
      .then(res => res.json(res))
      .then(data => {
        console.log(data)
        if (data?.authorizationToken) {
          Api.headers.Authorization = 'Bearer: ' + data.authorizationToken;
          setLogged(true);
        }
      })
      .catch(err => console.error(err));
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
        >
          Contraseña
        </TextField>
        <Button onPress={login}  >Ingresar</Button>
      </View>
    </Background>
  );
}