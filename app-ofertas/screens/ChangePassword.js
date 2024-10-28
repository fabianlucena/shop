import { View, Text } from 'react-native';
import Button from '../components/Button';
import Background from '../components/Background';
import FancyText from '../components/FancyText';
import styles from '../libs/styles';
import TextField from '../components/TextField';
import { useEffect, useState } from 'react';
import { Api } from '../libs/api';

export default function ChangePasswordScreen({ navigation }) {
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmation, setConfirmation] = useState('');
  const [hint, setHint] = useState('');
  const [canChangePassword, setCanChangePassword] = useState('');

  useEffect(() => {
    if (!currentPassword) {
      setHint('Debe proporcionar la contraseña actual.');
      setCanChangePassword(false);
    } else if (!newPassword) {
      setHint('Debe proporcionar una nueva contraseña.');
      setCanChangePassword(false);
    } else if (newPassword == currentPassword) {
      setHint('La contraseña actual y la nueva deben ser diferentes.');
      setCanChangePassword(false);
    } else if (newPassword != confirmation) {
      setHint('La confirmación y la nueva contraseña son distintas.');
      setCanChangePassword(false);
    } else {
      setHint('Listo para enviar');
      setCanChangePassword(true);
    }
  }, [currentPassword, newPassword, confirmation]);

  async function changePassword() {
    try {
      const data = await Api.postJson(
        '/v1/change-password',
        {
          body: {
            currentPassword,
            newPassword,
          }
        }
      );

      navigation.navigate('Home');
    } catch(err) {
      setHint('Ocurrió un error, no se pudo cambiar la contraseña');
    }
  }

  return (
    <Background>
      <View style={styles.container}>
        <FancyText>{hint}</FancyText>
        <TextField
          value={currentPassword}
          onChangeValue={setCurrentPassword}
          secureTextEntry={true}
        >
          Contraseña actual
        </TextField>
        <TextField
          value={newPassword}
          onChangeValue={setNewPassword}
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
        <Button
          disabled={!canChangePassword}
          onPress={changePassword}
        >
          Cambiar
        </Button>
      </View>
    </Background>
  );
}