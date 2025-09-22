import ListScreen from '../components/ListScreen';
import useStore from '../services/useStore';

export default function StoresListScreen() {
  return <ListScreen
      showCommerceName={true}
      service={useStore()}
      confirmDeletionMessage={item => `¿Desea eliminar el local ${item.name}?`}
      formScreen="StoreForm"
      loadOptions={{ query: { includeDisabled: true } }}
      elements={[
        {
          style: { width: '100%' },
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