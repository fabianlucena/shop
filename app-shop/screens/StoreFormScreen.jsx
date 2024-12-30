import { useState, useEffect } from 'react';

import Screen from '../components/Screen';
import TextField from '../components/TextField';
import SwitchField from '../components/SwitchField';
import Button from '../components/Button';
import Message from '../components/Message';
import Error from '../components/Error';

import useStore from '../services/useStore';

export default function StoreFormScreen({ navigation, route }) {
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const [isEnabled, setIsEnabled] = useState(true);
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const storeService = useStore();

  const uuid = route?.params?.uuid;

  useEffect(() => {
    navigation.setOptions({ title: uuid? 'Modificar local': 'Agregar local' });
  }, [navigation]);

  useEffect(() => {
    if (!uuid)
      return;
    
    setLoading(true);
    setMessage('Cargando local...');
    storeService.getSingleForUuid(uuid)
      .then(data => {
        setIsEnabled(data.isEnabled);
        setName(data.name);
        setDescription(data.description);
        setMessage('Local cargado.');
      })
      .catch(e => setError(`No se pudo cargar el local.\n${e.message}`))
      .finally(() => setLoading(false));
  }, []);

  useEffect(() => {
    if (loading) {
      setCanSubmit(false);
    } else if (!name) {
      setMessage('Debe proporcionar un nombre para el local.');
      setCanSubmit(false);
    } else if (!description) {
      setMessage('Debe proporcionar una descripción para el local.');
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
      storeService.updateForUuid(uuid, data)
        .then(() => {
          setMessage('Local actualizado correctamente.');
          navigation.navigate('Drawer', { screen: 'StoreesList'});
        })
        .catch(e => setError(`No se pudo actualizar el local.\n${e.message}`))
        .finally(() => setLoading(false));
    } else {
      setMessage('Creando...');
      storeService.add(data)
        .then(() => {
          setMessage('Local creado correctamente.');
          navigation.navigate('Drawer', { screen: 'StoreesList'});
        })
        .catch(e => setError(`No se pudo agregar el local.\n${e.message}`))
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