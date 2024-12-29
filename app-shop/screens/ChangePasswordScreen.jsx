import { useEffect, useState } from 'react';

import { Api } from '../libs/api';

import Button from '../components/Button';
import Screen from '../components/Screen';
import Message from '../components/Message';
import TextField from '../components/TextField';
import SuccessMessage from '../components/SuccessMessage';

export default function ChangePasswordScreen({ navigation }) {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmation, setConfirmation] = useState('');
  const [message, setMessage] = useState('');
  const [canChangePassword, setCanChangePassword] = useState('');

  useEffect(() => {
    if (!currentPassword) {
      setMessage('Debe proporcionar la contraseña actual.');
      setCanChangePassword(false);
    } else if (!newPassword) {
      setMessage('Debe proporcionar una nueva contraseña.');
      setCanChangePassword(false);
    } else if (newPassword == currentPassword) {
      setMessage('La contraseña actual y la nueva deben ser diferentes.');
      setCanChangePassword(false);
    } else if (newPassword != confirmation) {
      setMessage('La confirmación y la nueva contraseña son distintas.');
      setCanChangePassword(false);
    } else {
      setMessage('Listo para enviar');
      setCanChangePassword(true);
    }
  }, [currentPassword, newPassword, confirmation]);

  async function changePassword() {
    setLoading(true);
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

      setSuccess(true);
    } catch(err) {
      setMessage('Ocurrió un error, no se pudo cambiar la contraseña');
    }
    setLoading(false);
  }

  if (success) {
    return (
      <SuccessMessage
        onPress={() => navigation.navigate('Home')}
      >
        Contraseña cambiada satisfactoriamente
      </SuccessMessage>
    );    
  }

  return (
    <Screen
      busy={loading}
    >
      <Message>{message}</Message>
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
    </Screen>
  );
}