import ListScreen from '../components/ListScreen';
import useItem from '../services/useItem';
import styles from '../libs/styles';

export default function ExploreScreen() {
  return <ListScreen
      service={useItem()}
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
            ...styles.itemDescription,
            width: '100%',
          },
        },
      ]}
    />;
}