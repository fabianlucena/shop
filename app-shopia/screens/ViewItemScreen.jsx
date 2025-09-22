import { useEffect, useState } from 'react';
import { useRoute } from '@react-navigation/native';
import { Text, ScrollView, View } from 'react-native';

import useSession from '../components/Session';

import Screen from '../components/Screen';
import useItem from '../services/useItem';
import styles from '../libs/styles';
import { formatCurrency } from '../libs/format';
import ImageGaleryShow from '../components/ImageGaleryShow';

export default function ViewItemScreen({
  uuid
}) {
  const route = useRoute();
  const _uuid = uuid ?? route?.params?.uuid;
  const service = useItem();
  const [data, setData] = useState({});
  const [loading, setLoading] = useState(false);
  const { addError } = useSession();

  useEffect(() => {
    if (_uuid)
      loadData();
  }, [_uuid]);

  function loadData() {
    setLoading(true);
    service.getSingleForUuid(_uuid)
      .then(setData)
      .catch(e => addError(`Error al cargar el artículo.\n${e.message}`))
      .finally(() => setLoading(false));
  }

  return <Screen
      busy={loading}
    >
      <ScrollView
        style={{
          padding: 10,
          paddingTop: 0,
          width: '100%',
        }}
        contentContainerStyle={{
          alignItems: 'flex-start',
          justifyContent: 'flex-start',
        }}
      >
        <Text style={ styles.itemHeader }>{data.name}</Text>

        <Text style={ styles.itemLabel }>Descripción: </Text>
        <Text style={ styles.itemData }>{data.description}</Text>

        <Text style={ styles.itemLabel }>Rubro: </Text>
        <Text style={ styles.itemData }>{data.category?.name}</Text>

        <Text style={ styles.itemData }>Precio: {data.price ? formatCurrency(data.price) : 'No disponible'}</Text>

        <Text style={ styles.itemData }>Quedan: {typeof data.stock !== 'undefined' ? data.stock : 'Sin datos'}</Text>

        {data.isPresent ?
          data.minAge || data.minAge === 0 ?
            data.maxAge ?
              <Text style={ styles.itemData }>Para regalar de {data.minAge} a {data.maxAge} años</Text>
            : <Text style={ styles.itemData }>Para regalar a partir de {data.minAge} años</Text>
            : data.maxAge ?
              <Text style={ styles.itemData }>Para regalar hasta {data.maxAge} años</Text>
            : <Text style={ styles.itemData }>Para regalar todas las edades</Text>
        : null}

        <View
          style={{
            marginTop: '5%',
            marginLeft: '15%',
            marginRight: '15%',
          }}
        >
          <ImageGaleryShow
            images={data.images}
            style={{
              aspectRatio: 9 / 16,
            }}
          />
        </View>
      </ScrollView>
    </Screen>;
}