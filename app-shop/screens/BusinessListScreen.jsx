import { useEffect, useState } from 'react';
import { FlatList, View, Text, Alert } from 'react-native';

import styles from '../libs/styles';

import Screen from '../components/Screen';
import Button from '../components/Button';
import useDialog from '../components/useDialog';

import useBusiness from '../services/useBusiness';

export default function BusinessListScreen({ navigation }) {
  const [reload, setReload] = useState([]);
  const [data, setData] = useState([]);
  const businessService = useBusiness();
  const dialog = useDialog();

  useEffect(() => {
    businessService.get()
      .then(data => setData(data.rows));
  }, [reload]);


  function deleteRow({name, uuid}) {
    dialog.confirm({
      message: `¿Desea eliminar el negocio ${name}?`,
      onOk: () => businessService.deleteForUuid(uuid)
        .then(() => setReload(reload + 1))
        .catch(err => dialog.message(err)),
    });
  }

  function renderItem({ item }) {
    return <View
        style={styles.item}
      >
        <Text>{item.name}</Text>
        <Text>{item.isEnabled? 'Habilitado': 'Deshabilitado'}</Text>
        <Text>{item.description}</Text>
        <Button onPress={() => navigation.navigate('BusinessForm', { uuid: item.uuid })}>Modificar</Button>
        <Button onPress={() => deleteRow(item)}>Eliminar</Button>
      </View>;
  }

  return (
    <Screen style={{ backgroundColor: 'red' }}>
      <Button onPress={() => navigation.navigate('BusinessForm')} >Agregar</Button>
      <FlatList
        style= {{
          width: '100%',
          padding: '.5em',
        }}
        data={data}
        renderItem={renderItem}
        keyExtractor={item => item.uuid}
      />
    </Screen>
  );
}