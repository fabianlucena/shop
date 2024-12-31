import { useState, useEffect } from 'react';
import { Text } from 'react-native';
import { useNavigation, useRoute } from '@react-navigation/native';

import Screen from './Screen';
import TextField from './TextField';
import SwitchField from './SwitchField';
import Button from './Button';
import Message from './Message';
import Error from './Error';

function getArrangedFields(fields) {
  return fields.map(f => {
    if (f === 'isEnabled') {
      f = {
        name: 'isEnabled',
        type: 'switch',
        label: 'Habilitado',
      };
    } else if (f === 'name' || f === 'description') {
      f = {
        name: f,
        type: 'text',
        label: f === 'name'? 'Nombre': 'Descripción',
      };
    }

    return f;
  });
}

function renderFields(fields, data, setData) {
  if (!fields)
    return;

  return getArrangedFields(fields).map(f => {
    if (f.type === 'switch') {
      return <SwitchField
          key={f.name}
          value={data[f.name]}
          onChangeValue={value => setData({...data, [f.name]: value})}
        >
          {f.label}
        </SwitchField>;
    }

    if (f.type === 'text') {
      return <TextField
          key={f.name}
          value={data[f.name]}
          onChangeValue={value => {
            setData({...data, [f.name]: value});
          }}
        >
          {f.label}
        </TextField>;
    }
    
    return <Text key={f.name}>Tipo de campo desconocido {JSON.stringify(f)}</Text>
  });
}

export default function FormScreen({
  service,
  uuid,
  fields,
  createTitle = 'Agregar',
  updateTitle = 'Modificar',
  loadingError = 'Error de carga',
  createErrorMessage = 'No se pudo agregar.',
  updateErrorMessage = 'No se pudo actualizar.',
  onSuccessNavigate,
  validate,
}) {
  const navigation = useNavigation();
  const route = useRoute();
  const [data, setData] = useState({});
  const [defaultData, setDefaultData] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const _uuid = uuid ?? route?.params?.uuid;

  useEffect(() => {
    navigation.setOptions({ title: _uuid? updateTitle: createTitle });

    if (!_uuid)
      return;
    
    setLoading(true);
    setMessage('Cargando...');
    service.getSingleForUuid(_uuid)
      .then(d => {
        const f = {};
        for (var k of getArrangedFields(fields))
          f[k.name] = d[k.name];

        setData(f);
        setDefaultData(JSON.stringify(f));
      })
      .catch(e => setError(`${loadingError}\n${e.message}`))
      .finally(() => setLoading(false));
  }, [_uuid]);

  useEffect(() => {
    if (loading) {
      setCanSubmit(false);
      return;
    }
    
    if (validate) {
      const result = validate(data);

      if (result === false) {
        setMessage('Verifique los datos');
        setCanSubmit(false);
        return;
      }

      if (result === true) {
        setMessage('Listo para enviar');
        setCanSubmit(true);
        return;
      }

      if (result) {
        if (typeof result !== 'string')
          result = JSON.stringify(result);
    
        setMessage(result);
        setCanSubmit(false);
        return;
      }
    }

    if (JSON.stringify(data) === defaultData) {
      setMessage('No hay cambios para enviar');
      setCanSubmit(false);
      return;
    }

    setMessage('Listo para enviar');
    setCanSubmit(true);
  }, [loading, data]);

  function handleOnSuccess() {
    if (onSuccessNavigate && navigation) {
      if (Array.isArray(onSuccessNavigate))
        navigation.navigate(...onSuccessNavigate);
      else
        navigation.navigate(onSuccessNavigate);

      return;
    }
  }

  function handleOnError(err) {
    setError(`${_uuid? updateErrorMessage: createErrorMessage}\n${err.message}`)
  }

  function handleOnSubmit() {
    if (!canSubmit)
      return;

    setLoading(true);
    setError('');

    let messageText,
      method;
    if (_uuid) {
      messageText = 'Actualizando...';
      method = service.updateForUuid(_uuid, data);
    } else {
      messageText = 'Creando...';
      method = service.add(data);
    }

    setMessage(messageText);
    method
      .then(handleOnSuccess)
      .catch(handleOnError)
      .finally(() => setLoading(false));
  }

  return <Screen
      busy={loading}
    >
      <Error>{error}</Error>
      <Message>{message}</Message>
      {renderFields(fields, data, setData)}
      <Button
        disabled={!canSubmit}
        onPress={handleOnSubmit}
      >
        {_uuid? 'Modificar': 'Agregar'}
      </Button>
    </Screen>;
}