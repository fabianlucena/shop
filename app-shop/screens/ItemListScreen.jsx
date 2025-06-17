import ListScreen from '../components/ListScreen';
import useItem from '../services/useItem';

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
            { button:  'edit' },
            { button:  'delete' },
          ]
        },
        {field: 'description'},
      ]}
    />;
}