import { View } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import styles from '../libs/styles';
import TextField from '../components/TextField';
import { useState } from 'react';

export default function ChangePasswordScreen({ navigation }) {
  const [current, setCurrent] = useState('');
  const [password, setPassword] = useState('');
  const [confirmation, setConfirmation] = useState('');

  return (
    <Background>
      <View style={styles.container}>
        <TextField
          value={current}
          onChangeValue={setCurrent}
          secureTextEntry={true}
        >
          Contraseña actual
        </TextField>
        <TextField
          value={password}
          onChangeValue={setPassword}
          secureTextEntry={true}
        >
          Contraseña nueva
        </TextField>
        <TextField
          value={confirmation}
          onChangeValue={setConfirmation}
          secureTextEntry={true}
        >
          Confirme la contraseña
        </TextField>
        <View style={styles.sameLine}>
          <Button onPress={() => navigation.navigate('Register')} >Cambiar</Button>
        </View>
      </View>
    </Background>
  );
}