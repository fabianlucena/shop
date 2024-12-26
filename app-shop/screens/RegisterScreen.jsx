import { View, ActivityIndicator } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import TextField from '../components/TextField';
import Message from '../components/Message';
import { useState, useEffect } from 'react';
import { Api } from '../libs/api';
import SuccessMessage from '../components/SuccessMessage';
import Busy from '../components/Busy';

export default function RegisterScreen({ navigation }) {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [fullName, setFullName] = useState('');
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmation, setConfirmation] = useState('');
  const [email, setEmail] = useState('');
  const [message, setMessage] = useState('');
  const [canRegister, setCanRegister] = useState('');

  useEffect(() => {
    if (!fullName) {
      setMessage('Debe proporcionar su nombre completo.');
      setCanRegister(false);
    } else if (!username) {
      setMessage('Debe proporcionar un nombre de usuario.');
      setCanRegister(false);
    } else if (!password) {
      setMessage('Debe proporcionar una contraseña.');
      setCanRegister(false);
    } else if (password != confirmation) {
      setMessage('La confirmación y la contraseña son distintas.');
      setCanRegister(false);
    } else if (!email) {
      setMessage('Debe proporcionar un correo electrónico.');
      setCanRegister(false);
    } else if(!/^\w+([.-_+]?\w+)*@\w+([.-]?\w+)*(\.\w{2,10})+$/.test(email)) {
      setMessage('La dirección de correo electrónico no es válida.');
      setCanRegister(false);
    } else {
      setMessage('Listo para enviar');
      setCanRegister(true);
    }
  }, [fullName, username, password, confirmation, email]);

  async function register() {
    setLoading(true);
    try {
      await Api.postJson(
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

      setSuccess(true);
    } catch(err) {
      console.error(err);
      setMessage('Ocurrió un error, no se creó la cuenta');
    }
    setLoading(false);
  }

  if (success) {
    return (
      <SuccessMessage
        onPressMessage='Ir a ingresar'
        onPress={() => navigation.navigate('Login')}
      >
        Usuario registrado satisfactoriamente
      </SuccessMessage>
    );    
  }

  return (
    <Background>
      <Busy busy={loading}>
        <View style={styles.container}>
          <Message>{message}</Message>
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
      </Busy>
    </Background>
  );
}