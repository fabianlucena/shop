import ListScreen from '../components/ListScreen';
import useStore from '../services/useStore';
import { useSession } from '../components/Session';

export default function StoresListScreen() {
  const { loadStores } = useSession();

  return <ListScreen
      showCommerceName={true}
      service={useStore()}
      confirmDeletionMessage={item => `¿Desea eliminar el local ${item.name}?`}
      formScreen="StoreForm"
      loadOptions={{ query: { includeDisabled: true } }}
      onSuccess={loadStores}
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