import { useState, useEffect, useCallback } from 'react';
import { Text, KeyboardAvoidingView, Platform, ScrollView } from 'react-native';
import { useNavigation, useRoute } from '@react-navigation/native';

import { useSession } from '../contexts/Session';

import Screen from './Screen';
import TextField from './TextField';
import SwitchField from './SwitchField';
import SelectField from './SelectField';
import CurrencyField from './CurrencyField';
import ImageGaleryField from './ImageGaleryField';
import Button from './Button';
import Message from './Message';

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

    if (f.type === 'imageGalery') {
      return <ImageGaleryField
          key={f.name}
          name={f.name}
          value={data[f.name]?.map(image => (typeof image === 'string' ? { image } : image)) || []}
          setValue={images => setData({...data, [f.name]: images})}
          service={f.service}
        />;
    }
    
    return <Text key={f.name}>Tipo de campo desconocido {JSON.stringify(f)}</Text>
  });
}

export default function FormScreen({
  header,
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
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const _uuid = uuid ?? route?.params?.uuid;
  const [_fields, setFields] = useState([]);
  const { addError } = useSession();
  const [message, setMessage] = useState('');

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

    if (JSON.stringify(data) === originalData) {
      setMessage('No hay cambios para enviar');
      setCanSubmit(false);
      return;
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
      .catch(e => addError(`${loadingError}\n${e.message}`))
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
    addError(`${_uuid? updateErrorMessage: createErrorMessage}\n${err.message}`)
  }

  function getFieldByName(name) {
    return _fields.find(f => f.name === name);
  }

  function handleOnSubmit() {
    if (!canSubmit)
      return;

    setLoading(true);

    let sendData;
    if (_fields.some(f => (f.visible || typeof f.visible === 'undefined') && f.type === 'imageGalery' && data[f.name]?.length )) {
      sendData = new FormData();
      for (var name in data) {
        const value = data[name];
        const field = getFieldByName(name);
        if (field.type === 'imageGalery' && value?.length) {
          const rootName = name + '_';
          const deleted = [];
          for (var index in value) {
            const image = value[index];
            const uri = image.uri;
            if (!uri)
              continue;

            if (image.added) {
              const thisName = rootName + index;
              const ext = uri.split('.').pop().toLowerCase();
              let type;
              switch (ext) {
                case 'jpg':
                case 'jpeg':
                  type = 'image/jpeg';
                  break;
                case 'png':
                  type = 'image/png';
                  break;
                case 'webp':
                  type = 'image/webp';
                  break;
                case 'gif':
                  type = 'image/gif';
                  break;
                default:
                  type = 'application/octet-stream';
              }

              sendData.append(thisName, {
                uri,
                name: thisName + '.' + ext,
                type,
              });
            } else if (image.deleted) {
              if (image.uuid) {
                deleted.push(image.uuid);
              }
            }
          }

          if (deleted.length) {
            sendData.append(field.deleteFieldName ?? 'delete' + name, JSON.stringify(deleted));
          }
        } else {
          sendData.append(name, value);
        }
      }
    } else {
      sendData = data;
    }

    let messageText,
      method;
    if (_uuid) {
      messageText = 'Actualizando...';
      method = service.updateForUuid(_uuid, sendData);
    } else {
      messageText = 'Creando...';
      method = service.add(sendData);
    }

    setMessage(messageText);
    method
      .then(handleOnSuccess)
      .catch(handleOnError)
      .finally(() => setLoading(false));
  }

  return <KeyboardAvoidingView
      style={{ flex: 1 }}
      behavior={Platform.OS === "ios" ? "padding" : "height"}
    >
      <Screen
        header={header}
        busy={loading}
        showCommerceName={showCommerceName}
      >
        <Message>{message}</Message>
        <ScrollView
          contentContainerStyle={{ paddingBottom: 60 }}
          keyboardShouldPersistTaps="handled"
          nestedScrollEnabled={true}
          style={{
            flex: 1,
            width: '100%',
          }}
        >
          {renderFields(_fields, data, setData)}
          <Button
            disabled={!canSubmit}
            onPress={handleOnSubmit}
          >
            {_uuid? 'Modificar': 'Agregar'}
          </Button>
        </ScrollView>
      </Screen>
    </KeyboardAvoidingView>;
}