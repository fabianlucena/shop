import { useState, useCallback } from 'react';
import { FlatList, View, Text } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';

import styles from '../libs/styles';

import Screen from '../components/Screen';
import ItemHeader from '../components/ItemHeader';
import ButtonIconEdit from '../components/ButtonIconEdit';
import ButtonIconDelete from '../components/ButtonIconDelete';
import useDialog from '../components/useDialog';

import useStore from '../services/useStore';

export default function StoresListScreen({ navigation }) {
  const [reload, setReload] = useState([]);
  const [data, setData] = useState([]);
  const storeService = useStore();
  const dialog = useDialog();

  useFocusEffect(
    useCallback(() => {
      storeService.get()
        .then(data => setData(data.rows));
    }, [])
  );

  function deleteRow({name, uuid}) {
    dialog.confirm({
      message: `¿Desea eliminar el local ${name}?`,
      onOk: () => storeService.deleteForUuid(uuid)
        .then(() => setReload(reload + 1))
        .catch(err => dialog.message(err)),
    });
  }

  function renderItem({ item }) {
    return <View style={styles.item} >
        <ItemHeader>{item.name}</ItemHeader>
        <Text>{item.isEnabled? 'Habilitado': 'Deshabilitado'}</Text>
        <Text>{item.description}</Text>
        <View style={styles.sameLine} >
          <ButtonIconEdit navigate={['BusinessForm', { uuid: item.uuid }]} />
          <ButtonIconDelete onPress={() => deleteRow(item)} />
        </View>
      </View>;
  }

  return (
    <Screen style={{ backgroundColor: 'red' }}>
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