import { useState, useCallback } from 'react';
import { FlatList, View, Text, Switch, Pressable } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';

import styles from '../libs/styles';

import Screen from './Screen';
import ListItemHeader from './ListItemHeader';
import ButtonIconEdit from './ButtonIconEdit';
import ButtonIconDelete from './ButtonIconDelete';
import useDialog from './useDialog';
import Error from './Error';
import Currency from './Currency';
import ImageGaleryShow from './ImageGaleryShow';

export default function ListScreen({
  header,
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
  onPressItem,
  numColumns,
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
      .then(data => {
        const newData = data.rows;
        const rest = data.rows.length % numColumns;
        if (rest > 0) {
          const emptyRows = Array.from({ length: numColumns - rest }, () => (null));
          newData.push(...emptyRows);
        }
        setData(newData);
      })
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

  function renderElementType(element, item) {
    switch (element.type) {
      case 'currency':
        return <Currency
            style={element.style}
          >
            {item[element.field]}
          </Currency>;

      case 'image':
        return <ImageGaleryShow
            images={item[element.field]}
            style={element.style}
            service={service[element.endPoint]}
          />;
    }

    if (!element.field)
      return null;

    return <Text style={{...element.style}}>{item[element.field]}</Text>;
  }

  function renderElement(element, item) {
    return <View
      key={`${ element.name ?? element.field ?? element.fieldHeader ?? element.control ?? element.button }-${item.uuid}`}
      style={{
        ...styles.sameLine,
        ...(element.elements &&
        {
          display: 'flex',
          flexDirection: 'row',
          width: '100%',
        }),
      }} >
        {element.label && <Text style={styles.label}>{element.label}</Text> || null }
        {element.fieldHeader && <ListItemHeader >{item[element.fieldHeader]}</ListItemHeader> || null}
        {renderElementType(element, item)}
        {element.elements && renderElements(element.elements, item) || null}
        {element.button && renderButton(element.button, item) || null}
        {element.control && renderControl(element.control, item) || null}
        {!element.field && !element.fieldHeader && !element.elements && !element.button && !element.control
          && <Text>Elemento desconocido: {JSON.stringify(element)}</Text> || null
        }
      </View>;
  }

  function renderElements(elements, item) {
    return <>
        {elements?.map(element => renderElement(element, item))}
      </>;
  }

  function renderItem({ item }) {
    if (!item) {
      return <View style={styles.listItemEmpty} key="empty-item" />;
    }

    return <Pressable
        key={item.uuid}
        onPress={() => onPressItem && onPressItem(item)}
        style={styles.listItem}
      >
        {renderElements(elements, item)}
      </Pressable>;
  }

  return <Screen
      header={header}
      showCommerceName={showCommerceName}
    >
      <Error>{error}</Error>
      <FlatList
        style={styles.list}
        data={data}
        renderItem={renderItem}
        keyExtractor={item => item?.uuid}
        numColumns={numColumns}
      />
    </Screen>;
}