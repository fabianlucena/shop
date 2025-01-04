import ListScreen from '../components/ListScreen';
import useCommerce from '../services/useCommerce';
import { useSession } from '../contexts/Session';

export default function CommercesListScreen() {
  const { loadCommerce } = useSession();

  return <ListScreen
      service={useCommerce()}
      confirmDeletionMessage={item => `¿Desea eliminar el comercio ${item.name}?`}
      formScreen="CommerceForm"
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
      onDeleted={loadCommerce}
      onEnabled={loadCommerce}
    />;
}