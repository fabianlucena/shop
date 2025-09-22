import ListScreen from '../components/ListScreen';
import useCommerce from '../services/useCommerce';
import { useSession } from '../components/Session';

export default function CommercesListScreen() {
  const { loadCommerces } = useSession();

  return <ListScreen
      service={useCommerce()}
      confirmDeletionMessage={item => `¿Desea eliminar el comercio ${item.name}?`}
      formScreen="CommerceForm"
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
      onDeleted={loadCommerces}
      onEnabled={loadCommerces}
    />;
}