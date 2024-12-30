import { useState, useCallback } from 'react';
import { FlatList, View, Text } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';

import styles from '../libs/styles';

import Screen from '../components/Screen';
import ItemHeader from '../components/ItemHeader';
import ButtonIconEdit from '../components/ButtonIconEdit';
import ButtonIconDelete from '../components/ButtonIconDelete';
import useDialog from '../components/useDialog';

import useBusiness from '../services/useBusiness';

export default function BusinessesListScreen({ navigation }) {
  const [reload, setReload] = useState(0);
  const [data, setData] = useState([]);
  const businessService = useBusiness();
  const dialog = useDialog();

  useFocusEffect(
    useCallback(() => {
      businessService.get()
        .then(data => setData(data.rows));
    }, [])
  );

  function deleteRow({name, uuid}) {
    dialog.confirm({
      message: `¿Desea eliminar el negocio ${name}?`,
      onOk: () => businessService.deleteForUuid(uuid)
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