import { useState, useEffect } from 'react';

import Screen from '../components/Screen';
import TextField from '../components/TextField';
import SelectField from '../components/SelectField';
import Button from '../components/Button';
import Message from '../components/Message';
import CurrencyField from '../components/CurrencyField';
import SwitchField from '../components/SwitchField';

export default function ItemFormScreen({ uuid}) {
  const [message, setMessage] = useState('');
  const [canSubmit, setCanSubmit] = useState(false);
  const [loading, setLoading] = useState(false);
  const [name, setName] = useState('');
  const [category, setCategory] = useState('');
  const [price, setPrice] = useState(0);
  const [isPresent, setIsPresent] = useState(false);
  const [minAge, setMinAge] = useState(0);
  const [maxAge, setMaxAge] = useState(0);

  const categories = [
    { label: 'Opción 1', value: 'opcion1' },
    { label: 'Opción 2', value: 'opcion2' },
    { label: 'Opción 3', value: 'opcion3' },
  ];

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
        value={category}
        onChangeValue={setCategory}
        options={categories}
      >
        Rubro
      </SelectField>
      <CurrencyField
        value={price}
        onChangeValue={setPrice}
      >
        Precio
      </CurrencyField>
      <SwitchField
        value={isPresent}
        onChangeValue={setIsPresent}
      >
        Apto para regalar
      </SwitchField>
      {isPresent && <>
        <TextField
          value={minAge}
          onChangeValue={setMinAge}
          keyboardType="numeric"
        >
          Edad mínima
        </TextField>
        <TextField
          value={maxAge}
          onChangeValue={setMaxAge}
          keyboardType="numeric"
        >
          Edad máxima
        </TextField>
      </>}
      <Button
        disabled={!canSubmit}
        onPress={submit}
      >
        {uuid? 'Modificar': 'Agregar'}
      </Button>
    </Screen>
  );
}