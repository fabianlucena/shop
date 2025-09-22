import ListScreen from '../components/ListScreen';
import useCommerce from '../services/useCommerce';
import { useSession } from '../components/Session';

export default function CommercesListScreen() {
  const { loadCommerces } = useSession();

  return <ListScreen
      service={useCommerce()}
      confirmDeletionMessage={item => `¿Desea eliminar el comercio ${item.name}?`}
      formScreen="CommerceForm"
      loadOptions={{ query: { includeDisabled: true } }}
      onSuccess={loadCommerces}
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