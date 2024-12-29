import { useState, useEffect } from 'react';

import Screen from '../components/Screen';
import TextField from '../components/TextField';
import Button from '../components/Button';
import Message from '../components/Message';

import useCompany from '../services/useCompany';

export default function CompanyFormScreen({ uuid }) {
  const [message, setMessage] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const companyService = useCompany();

  useEffect(() => {
    if (loading) {
      setMessage('Cargando.');
      setCanSubmit(false);
    } else if (!name) {
      setMessage('Debe proporcionar un nombre para la empresa.');
      setCanSubmit(false);
    } else if (!description) {
      setMessage('Debe proporcionar una descripción para la empresa.');
      setCanSubmit(false);
    } else {
      setMessage('Listo para enviar');
      setCanSubmit(true);
    }
  }, [loading, name, description]);

  function submit() {
    setLoading(true);
    companyService.add({
      name,
      description,
    })
    .then(() => setMessage('Empresa creada correctamente.'))
    .catch(() => setMessage('Error al crear la empresa.'))
    .finally(() => setLoading(false));
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