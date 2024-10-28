import { View } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import TextField from '../components/TextField';
import { useState } from 'react';

export default function RegisterScreen({ navigation }) {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmation, setConfirmation] = useState('');
  const [email, setEmail] = useState('');

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
        <View style={styles.sameLine}>
          <Button onPress={() => navigation.navigate('Register')} >Registrarse</Button>
        </View>
      </View>
    </Background>
  );
}