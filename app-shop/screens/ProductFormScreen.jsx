import { useState, useEffect } from 'react';

import Screen from '../components/Screen';
import TextField from '../components/TextField';
import Button from '../components/Button';
import Message from '../components/Message';

export default function FormProductScreen() {
  const [message, setMessage] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const [name, setName] = useState('');

  useEffect(() => {
    if (loading) {
      setMessage('Cargando.');
      setCanSubmit(false);
    } else if (!name) {
      setMessage('Debe proporcionar su nombre para el artículo.');
      setCanSubmit(false);
    } else {
      setMessage('Listo para enviar');
      setCanSubmit(true);
    }
  }, [loading, name]);

  function submit() {
    console.log('submit', name);
  }

  return (
    <Screen
      busy={loading}
    >
      <Message>{message}</Message>
      <TextField
        value={name}
        onChangeValue={setName}
      >
        Nombre
      </TextField>
      <SelectField
        value={name}
        onChangeValue={setName}
      >
        Nombre
      </SelectField>
      <Button
        disabled={!canSubmit}
        onPress={submit}
      >
        Registrarse
      </Button>
    </Screen>
  );
}