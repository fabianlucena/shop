import { useState, useCallback, useEffect } from 'react';
import { FlatList, View, Text, Switch } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';

import styles from '../libs/styles';

import Screen from './Screen';
import ItemHeader from './ItemHeader';
import ButtonIconEdit from './ButtonIconEdit';
import ButtonIconDelete from './ButtonIconDelete';
import useDialog from './useDialog';
import Error from './Error';

export default function ListScreen({
  confirmDeletionMessage,
  service,
  elements,
  formScreen,
  onDelete,
  onDeleted,
  onEnable,
  onEnabled,
  showCommerceName,
  loadingError = 'Error de carga',
  loadOptions = {},
}) {
  const [data, setData] = useState([]);
  const dialog = useDialog();
  const [error, setError] = useState('');

  useFocusEffect(
    useCallback(() => {
      setError('');
      loadData();
    }, [])
  );
  
  function loadData() {
    service.get(loadOptions?.query)
      .then(data => setData(data.rows))
      .catch(e => setError(`${loadingError}\n${e.message}`));
  }

  function deleteRow(item) {
    dialog.confirm({
      message: confirmDeletionMessage(item) ?? '¿Desea eliminar el elemento?',
      onOk: () => {
        if (!onDelete || onDelete(item)) {
          service.deleteForUuid(item.uuid)
            .then(() => {
              onDeleted && onDeleted(item);
              loadData();
            })
            .catch(err => dialog.message(err));
          }
        },
    });
  }

  function renderButton(button, item) {
    if (!button)
      return;

    if (button === 'edit')
      return <ButtonIconEdit navigate={[formScreen, { uuid: item.uuid }]} />;
    
    if (button === 'delete')
      return <ButtonIconDelete onPress={() => deleteRow(item)} />;

    return <Text key={button.name}>Boton desconocido: {JSON.stringify(button)}</Text>;
  }

  function renderControl(control, item) {
    if (!control)
      return;

    if (control === 'isEnabled')
      return <Switch
          value={item.isEnabled}
          onValueChange={value => {
            if (!onEnable || onEnable(item)) {
              service.updateForUuid(item.uuid, { isEnabled: value })
                .then(() => {
                  onEnabled && onEnabled(value);
                  loadData();
                })
                .catch(err => dialog.message(err));
              };
            }
        }
        />;

    return <Text key={control.name}>Control desconocido: {JSON.stringify(control)}</Text>;
  }

  function renderElement(element, item) {
    return <View
      key={`${ element.name ?? element.field ?? element.fieldHeader ?? element.control ?? element.button }-${item.uuid}`}
      style={{
        ...styles.sameLine,
        ...element.style,
        ...(element.elements &&
        {
          display: 'flex',
          flexDirection: 'row',
          width: '100%',
        }),
        ...((element.fieldHeader || element.field || element.elements) &&
        {
          flexGrow: 1,
        }),
      }} >
        {element.fieldHeader && <ItemHeader key={element.name ?? element.fieldHeader} style={{flexGrow: 1}}>{item[element.fieldHeader]}</ItemHeader> || null}
        {element.field && <Text key={element.name ?? element.field}>{item[element.field]}</Text> || null}
        {element.elements && renderElements(element.elements, item) || null}
        {element.button && renderButton(element.button, item) || null}
        {element.control && renderControl(element.control, item) || null}
        {!element.field && !element.fieldHeader && !element.elements && !element.button && !element.control
          && <Text key={element.name}>Elemento desconocido: {JSON.stringify(element)}</Text> || null
        }
      </View>;
  }

  function renderElements(elements, item) {
    return <>
        {elements?.map(element => renderElement(element, item))}
      </>;
  }

  function renderItem({ item }) {
    return <View
        key={item.uuid}
        style={styles.item}
      >
        {renderElements(elements, item)}
      </View>;
  }

  return <Screen
      showCommerceName={showCommerceName}
    >
      <Error>{error}</Error>
      <FlatList
        style= {{
          width: '100%',
          padding: '10',
          flexDirection: 'column',
          gap: '.5em',
        }}
        data={data}
        renderItem={renderItem}
        keyExtractor={item => item.uuid}
      />
    </Screen>;
}