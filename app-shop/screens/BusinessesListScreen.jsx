import ListScreen from '../components/ListScreen';
import useBusiness from '../services/useBusiness';

export default function BusinessesListScreen() {
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
    />;
}