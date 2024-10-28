import { View } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import TextField from '../components/TextField';
import Hint from '../components/Hint';
import { useState, useEffect } from 'react';
import { Api } from '../libs/api';

export default function RegisterScreen({ navigation }) {
  const [fullName, setFullName] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmation, setConfirmation] = useState('');
  const [email, setEmail] = useState('');
  const [hint, setHint] = useState('');
  const [canRegister, setCanRegister] = useState('');

  useEffect(() => {
    if (!fullName) {
      setHint('Debe proporcionar su nombre completo.');
      setCanRegister(false);
    } else if (!username) {
      setHint('Debe proporcionar un nombre de usuario.');
      setCanRegister(false);
    } else if (!password) {
      setHint('Debe proporcionar una contraseña.');
      setCanRegister(false);
    } else if (password != confirmation) {
      setHint('La confirmación y la contraseña son distintas.');
      setCanRegister(false);
    } else if (!email) {
      setHint('Debe proporcionar un correo electrónico.');
      setCanRegister(false);
    } else if(!/^\w+([.-_+]?\w+)*@\w+([.-]?\w+)*(\.\w{2,10})+$/.test(email)) {
      setHint('La dirección de correo electrónico no es válida.');
      setCanRegister(false);
    } else {
      setHint('Listo para enviar');
      setCanRegister(true);
    }
  }, [fullName, username, password, confirmation, email]);

  async function register() {
    try {
      const data = await Api.postJson(
        '/v1/register',
        {
          body: {
            fullName,
            username,
            password,
            email,
          }
        }
      );

      navigation.navigate('Login');
    } catch(err) {
      console.error(err);
      setHint('Ocurrió un error, no se creó la cuenta');
    }
  }

  return (
    <Background>
      <View style={styles.container}>
        <Hint>{hint}</Hint>
        <TextField
          value={fullName}
          onChangeValue={setFullName}
        >
          Su nombre completo
        </TextField>
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
        <TextField
          value={confirmation}
          onChangeValue={setConfirmation}
          secureTextEntry={true}
        >
          Confirme la contraseña
        </TextField>
        <TextField
          value={email}
          onChangeValue={setEmail}
        >
          Correo electrónico
        </TextField>
        <Button
          disabled={!canRegister}
          onPress={register}
        >
          Registrarse
        </Button>
      </View>
    </Background>
  );
}