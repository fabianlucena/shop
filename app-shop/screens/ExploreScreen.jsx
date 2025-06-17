import { View, Text } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';

import ListScreen from '../components/ListScreen';
import useItem from '../services/useItem';
import Background from '../components/Background';
import styles from '../libs/styles';

export default function ExploreScreen() {
  const insets = useSafeAreaInsets();

  return <Background>
      <View style={[
          styles.screen,
          {
            paddingTop: insets.top,
            paddingBottom: insets.bottom,
            paddingLeft: insets.left,
            paddingRight: insets.right,
          }
        ]}>
      <Text>Explorando artículos</Text>
      <ListScreen
        service={useItem()}
        elements={[
          {
            name: 'items',
            elements: [
              { fieldHeader: 'name' },
              { field: 'price', label: 'Precio: ', type: 'currency' },
              { field: 'stock', label: 'Cantidad: ', type: 'number' },
            ]
          },
          {
            field: 'description',
            style: {
              ...styles.itemDescription,
              width: '100%',
            },
          },
        ]}
      />
        </View>
    </Background>;
}