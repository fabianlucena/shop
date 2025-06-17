import ListScreen from '../components/ListScreen';
import useItem from '../services/useItem';
import styles from '../libs/styles';

export default function ItemsListScreen() {
  return <ListScreen
      service={useItem()}
      confirmDeletionMessage={item => `¿Desea eliminar el artículo ${item.name}?`}
      formScreen="ItemForm"
      loadOptions={{ query: { isEnabled: true } }}
      elements={[
        {
          name: 'items',
          elements: [
            { control: 'isEnabled' },
            { fieldHeader: 'name' },
            { field: 'price', type: 'currency' },
            { field: 'stock', label: 'Quedan: ', type: 'number' },
            { button:  'edit' },
            { button:  'delete' },
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