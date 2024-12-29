import { useEffect, useState } from 'react';
import { FlatList, View, Text } from 'react-native';

import Screen from '../components/Screen';
import Button from '../components/Button';

import useCompany from '../services/useCompany';

export default function CompanyListScreen({ navigation}) {
  const [data, setData] = useState([]);
  const companyService = useCompany();

  useEffect(() => {
    companyService.get()
      .then(data => setData(data.rows));
  }, []);

  function renderItem({ item }) {
    return <View>
        <Text>{item.name}</Text>
      </View>;
  }

  return (
    <Screen>
      <Button onPress={() => navigation.navigate('CompanyForm')} >Agregar</Button>
      <FlatList
        data={data}
        renderItem={renderItem}
        keyExtractor={item => item.uuid}
      />
    </Screen>
  );
}