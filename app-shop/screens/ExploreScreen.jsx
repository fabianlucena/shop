import ListScreen from '../components/ListScreen';
import useItem from '../services/useItem';
import styles from '../libs/styles';
import { useNavigation } from '@react-navigation/native';

export default function ExploreScreen() {
  const navigation = useNavigation();

  return <ListScreen
      service={useItem()}
      onPressItem={item => navigation.navigate('ViewItem', { uuid: item.uuid })}
      numColumns={1}
      elements={[
        {
          name: 'items',
          elements: [
            { fieldHeader: 'name' },
            { field: 'price', label: 'Precio: ', type: 'currency' },
            { field: 'stock', label: 'Quedan: ', type: 'number' },
          ]
        },
        {
          field: 'description',
          style: {
            ...styles.listItemDescription,
            width: '100%',
          },
        },
      ]}
    />;
}