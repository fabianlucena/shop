import { useEffect, useState } from 'react';
import { FlatList, View, Text } from 'react-native';

import styles from '../libs/styles';

import Screen from '../components/Screen';
import Button from '../components/Button';

import useBusiness from '../services/useBusiness';

export default function BusinessListScreen({ navigation }) {
  const [data, setData] = useState([]);
  const businessService = useBusiness();

  useEffect(() => {
    businessService.get()
      .then(data => setData(data.rows));
  }, []);

  function renderItem({ item }) {
    return <View
        style={styles.item}
      >
        <Text>{item.name}</Text>
        <Text>{item.description}</Text>
        <Button>Modificar</Button>
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