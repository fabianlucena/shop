import ListScreen from '../components/ListScreen';
import useStore from '../services/useStore';

export default function StoresListScreen() {
  return <ListScreen
      service={useStore()}
      confirmDeletionMessage={item => `¿Desea eliminar el local ${item.name}?`}
      elements={[
        {
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