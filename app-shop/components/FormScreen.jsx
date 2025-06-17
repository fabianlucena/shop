import { useState, useEffect, useCallback } from 'react';
import { Text } from 'react-native';
import { useNavigation, useRoute, useFocusEffect } from '@react-navigation/native';

import Screen from './Screen';
import TextField from './TextField';
import SwitchField from './SwitchField';
import SelectField from './SelectField';
import CurrencyField from './CurrencyField';
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
        numberOfLines: (f === 'description')? 3: 1,
      };
    }

    f.label ||= f.name;

    return f;
  });
}

function renderFields(fields, data, setData) {
  if (!fields)
    return;

  return getArrangedFields(fields).map(f => {
    if (!f.visible && typeof f.visible !== 'undefined')
      return;

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
          numberOfLines={f.numberOfLines}
        >
          {f.label}
        </TextField>;
    }

    if (f.type === 'select') {
      return <SelectField
          key={f.name}
          value={data[f.name]}
          onChangeValue={value => {
            setData({...data, [f.name]: value});
          }}
          options={f.options}
        >
          {f.label}
        </SelectField>;
    }

    if (f.type === 'currency') {
      return <CurrencyField
          key={f.name}
          value={data[f.name]}
          onChangeValue={value => {
            setData({...data, [f.name]: value});
          }}
        >
          {f.label}
        </CurrencyField>;
    }

    if (f.type === 'number') {
      return <TextField
          key={f.name}
          keyboardType="numeric"
          value={data[f.name]}
          onChangeValue={value => {
            setData({...data, [f.name]: value.replace(/[^0-9]/g, '')});
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
  defaultData,
  additionalData,
  createTitle = 'Agregar',
  updateTitle = 'Modificar',
  loadingError = 'Error de carga',
  createErrorMessage = 'No se pudo agregar.',
  updateErrorMessage = 'No se pudo actualizar.',
  onSuccess,
  onSuccessNavigate,
  validate,
  showCommerceName,
}) {
  const navigation = useNavigation();
  const route = useRoute();
  const [data, setData] = useState({...defaultData, ...additionalData});
  const [originalData, setOriginalData] = useState('');
  const [message, setMessage] = useState('');
  const [error, setError] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const _uuid = uuid ?? route?.params?.uuid;
  const [_fields, setFields] = useState([]);

  useFocusEffect(
    useCallback(() => {
      setError('');
    }, [])
  );

  useEffect(() => {
    setFields(getArrangedFields(fields));
  }, [fields]);

  useEffect(() => {
    navigation.setOptions({ title: _uuid? updateTitle: createTitle });

    const newData = {
      ...defaultData,
      ...additionalData,
    };
    for (var field of _fields) {
      if (field.name === 'isEnabled')
        newData.isEnabled = true;
    }
    setData(newData);

    if (_uuid)
      loadData();
  }, [_uuid, additionalData]);

  useEffect(() => {
    if (loading) {
      setCanSubmit(false);
      return;
    }

    if (JSON.stringify(data) === originalData) {
      setMessage('No hay cambios para enviar');
      setCanSubmit(false);
      return;
    }
    
    if (validate) {
      const priorFields = JSON.stringify(_fields);
      const result = validate(data, _fields || []);
      if (JSON.stringify(_fields) !== priorFields)
        setFields(getArrangedFields(_fields));

      if (result === false) {
        setMessage('Verifique los datos');
        setCanSubmit(false);
        return;
      }

      if (result && result !== true) {
        if (typeof result !== 'string')
          result = JSON.stringify(result);
    
        setMessage(result);
        setCanSubmit(false);
        return;
      }
    }

    setMessage('Listo para enviar');
    setCanSubmit(true);
  }, [loading, _fields, data, additionalData]);

  function loadData() {
    setLoading(true);
    setMessage('Cargando...');
    service.getSingleForUuid(_uuid, { query: { includeDisabled: true }})
      .then(rawData => {
        const newData = {...additionalData};
        if (_fields.length)
        {
          for (var field of _fields)
            newData[field.name] = rawData[field.name];
        } else {
          for (var field of getArrangedFields(fields))
            newData[field.name] = rawData[field.name];
        }

        setData(newData);
        setOriginalData(JSON.stringify(newData));
      })
      .catch(e => setError(`${loadingError}\n${e.message}`))
      .finally(() => setLoading(false));
  }

  function handleOnSuccess() {
    onSuccess && onSuccess();

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
      showCommerceName={showCommerceName}
    >
      <Error>{error}</Error>
      <Message>{message}</Message>
      {renderFields(_fields, data, setData)}
      <Button
        disabled={!canSubmit}
        onPress={handleOnSubmit}
      >
        {_uuid? 'Modificar': 'Agregar'}
      </Button>
    </Screen>;
}