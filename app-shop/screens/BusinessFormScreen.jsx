import { useState, useEffect } from 'react';

import Screen from '../components/Screen';
import TextField from '../components/TextField';
import Button from '../components/Button';
import Message from '../components/Message';
import Error from '../components/Error';

import useBusiness from '../services/useBusiness';

export default function BusinessFormScreen({ uuid }) {
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const businessService = useBusiness();

  useEffect(() => {
    if (loading) {
      setCanSubmit(false);
    } else if (!name) {
      setMessage('Debe proporcionar un nombre para el negocio.');
      setCanSubmit(false);
    } else if (!description) {
      setMessage('Debe proporcionar una descripción para el negocio.');
      setCanSubmit(false);
    } else {
      setMessage('Listo para enviar');
      setCanSubmit(true);
    }
  }, [loading, name, description]);

  function submit() {
    setLoading(true);
    setError('');
    setMessage(uuid? 'Actualizando...': 'Agregando...');
    businessService.add({
      name,
      description,
    })
    .then(() => {
      setMessage('Negocio creado correctamente.');
      navigator.navigate('BusinessList');
    })
    .catch(e => setError(`No se pudo ${uuid? 'actualizar': 'agregar'} el negocio.\n${e.message}`))
    .finally(() => setLoading(false));
  }

  return (
    <Screen
      busy={loading}
    >
      <Error>{error}</Error>
      <Message>{message}</Message>
      <TextField
        value={name}
        onChangeValue={setName}
      >
        Nombre
      </TextField>
      <TextField
        value={description}
        onChangeValue={setDescription}
        multiline={true}
      >
        Descripción
      </TextField>
      <Button
        disabled={!canSubmit}
        onPress={submit}
      >
        {uuid? 'Modificar': 'Agregar'}
      </Button>
    </Screen>
  );
}