import ListScreen from '../components/ListScreen';
import useBusiness from '../services/useBusiness';
import { useSession } from '../contexts/Session';

export default function BusinessesListScreen() {
  const { loadBussiness } = useSession();

  return <ListScreen
      service={useBusiness()}
      confirmDeletionMessage={item => `¿Desea eliminar el negocio ${item.name}?`}
      properties={[
        'name',
        'isEnabled',
        'description',
      ]}
      buttons={[
        'edit',
        'delete',
      ]}
      onDelete={loadBussiness}
      onEnable={loadBussiness}
    />;
}