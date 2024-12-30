import { useState, useEffect } from 'react';

import Screen from '../components/Screen';
import TextField from '../components/TextField';
import SwitchField from '../components/SwitchField';
import Button from '../components/Button';
import Message from '../components/Message';
import Error from '../components/Error';

import useBusiness from '../services/useBusiness';

export default function BusinessFormScreen({ navigation, route }) {
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const [isEnabled, setIsEnabled] = useState(true);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const businessService = useBusiness();

  const uuid = route?.params?.uuid;

  useEffect(() => {
    navigation.setOptions({ title: uuid? 'Modificar negocio': 'Agregar negocio' });
  }, [navigation]);

  useEffect(() => {
    if (!uuid)
      return;
    
    setLoading(true);
    setMessage('Cargando negocio...');
    businessService.getSingleForUuid(uuid)
      .then(data => {
        setIsEnabled(data.isEnabled);
        setName(data.name);
        setDescription(data.description);
        setMessage('Negocio cargado.');
      })
      .catch(e => setError(`No se pudo cargar el negocio.\n${e.message}`))
      .finally(() => setLoading(false));
  }, []);

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
    const data = {
      isEnabled,
      name,
      description,
    };

    if (uuid) {
      setMessage('Actualizando...');
      businessService.updateForUuid(uuid, data)
        .then(() => {
          setMessage('Negocio actualizado correctamente.');
          navigation.navigate('Drawer', { screen: 'BusinessList'});
        })
        .catch(e => setError(`No se pudo actualizar el negocio.\n${e.message}`))
        .finally(() => setLoading(false));
    } else {
      setMessage('Creando...');
      businessService.add(data)
        .then(() => {
          setMessage('Negocio creado correctamente.');
          navigation.navigate('Drawer', { screen: 'BusinessList'});
        })
        .catch(e => setError(`No se pudo agregar el negocio.\n${e.message}`))
        .finally(() => setLoading(false));
    }
  }

  return (
    <Screen
      busy={loading}
    >
      <Error>{error}</Error>
      <Message>{message}</Message>
      <SwitchField
        value={isEnabled}
        onChangeValue={setIsEnabled}
      >
        Habilitado
      </SwitchField>
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