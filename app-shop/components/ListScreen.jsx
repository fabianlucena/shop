import { useState, useCallback, useEffect } from 'react';
import { FlatList, View, Text } from 'react-native';
import { useNavigation, useFocusEffect } from '@react-navigation/native';

import { useSession } from '../contexts/Session';

import styles from '../libs/styles';

import Screen from './Screen';
import ItemHeader from './ItemHeader';
import ButtonIconEdit from './ButtonIconEdit';
import ButtonIconDelete from './ButtonIconDelete';
import useDialog from './useDialog';

export default function ListScreen({
  confirmDeletionMessage,
  service,
  properties,
  buttons,
  onDelete,
  onEnable,
  title,
}) {
  const { businessName } = useSession();
  const navigation = useNavigation();
  const [data, setData] = useState([]);
  const dialog = useDialog();

  useEffect(() => {
    if (businessName && title)
      navigation.setOptions({ headerTitle: businessName + ' - ' + title});
  }, [title, businessName]);

  function loadData() {
    service.get()
      .then(data => setData(data.rows));
  }

  useFocusEffect(
    useCallback(loadData, [])
  );

  function deleteRow(item) {
    dialog.confirm({
      message: confirmDeletionMessage(item) ?? '¿Desea eliminar el elemento?',
      onOk: () => service.deleteForUuid(item.uuid)
        .then(() => {
          onDelete && onDelete();
          loadData();
        })
        .catch(err => dialog.message(err)),
    });
  }

  function renderProperties(item) {
    if (!properties)
      return;

    return <>
        {properties.map(p => {
          if (p === 'name')
            return <ItemHeader key={p}>{item.name}</ItemHeader>;
          else if (p === 'isEnabled')
            return <Text key={p}>{item.isEnabled? 'Habilitado': 'Deshabilitado'}</Text>;
          else if (typeof p === 'string')
            return <Text key={p}>{item[p]}</Text>;
          else if (typeof p === 'function')
            return p(item);
          else
            return p;
        })}
      </>;
  }

  function renderButtons(item) {
    if (!buttons)
      return;

    return <View style={styles.sameLine} >
        {buttons.map(b => {
          if (b === 'edit')
            return <ButtonIconEdit key={b} navigate={['BusinessForm', { uuid: item.uuid }]} />;
          else if (b === 'delete')
            return <ButtonIconDelete key={b} onPress={() => deleteRow(item)} />;
          else if (typeof b === 'function')
            return b(item);
          else
            return p;
        })}
      </View>;
  }

  function renderItem({ item }) {
    return <View style={styles.item} >
        {renderProperties(item)}
        {renderButtons(item)}
      </View>;
  }

  return <Screen style={{ backgroundColor: 'red' }}>
      <FlatList
        style= {{
          width: '100%',
          padding: '.5em',
        }}
        data={data}
        renderItem={renderItem}
        keyExtractor={item => item.uuid}
      />
    </Screen>;
}