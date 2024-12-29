import { useEffect, useState } from 'react';
import { FlatList, View, Text } from 'react-native';

import Screen from '../components/Screen';
import Button from '../components/Button';

import useBusiness from '../services/useBusiness';

export default function BusinessListScreen({ navigation}) {
  const [data, setData] = useState([]);
  const businessService = useBusiness();

  useEffect(() => {
    businessService.get()
      .then(data => setData(data.rows));
  }, []);

  function renderItem({ item }) {
    return <View>
        <Text>{item.name}</Text>
      </View>;
  }

  return (
    <Screen>
      <Button onPress={() => navigation.navigate('BusinessForm')} >Agregar</Button>
      <FlatList
        data={data}
        renderItem={renderItem}
        keyExtractor={item => item.uuid}
      />
    </Screen>
  );
}