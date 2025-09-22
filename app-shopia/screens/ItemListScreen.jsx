import ListScreen from '../components/ListScreen';
import useItem from '../services/useItem';
import styles from '../libs/styles';

export default function ItemsListScreen() {
  return <ListScreen
      service={useItem()}
      confirmDeletionMessage={item => `¿Desea eliminar el artículo ${item.name}?`}
      formScreen="ItemForm"
      loadOptions={{ query: { includeDisabled: true } }}
      elements={[
        {
          name: 'items',
          style: { width: '100%' },
          elements: [
            { control: 'isEnabled' },
            { fieldHeader: 'name', style: { flexGrow: 1 }},
            { field: 'price', type: 'currency' },
            { field: 'stock', label: '#', type: 'number' },
            {
              field: 'buttons',
              elements: [
                { button: 'edit' },
                { button: 'delete' },
              ],
            },
          ],
        },
        {
          field: 'description',
          style: styles.listItemDescription,
        },
      ]}
    />;
}