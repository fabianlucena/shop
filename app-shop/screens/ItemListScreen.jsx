import ListScreen from '../components/ListScreen';
import useItem from '../services/useItem';

export default function ItemsListScreen() {
  return <ListScreen
      service={useItem()}
      confirmDeletionMessage={item => `¿Desea eliminar el artículo ${item.name}?`}
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