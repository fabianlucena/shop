import FormScreen from '../components/FormScreen';
import useStore from '../services/useStore';
import { useSession } from '../contexts/Session';

export default function StoreFormScreen() {
  const { commerceUuid } = useSession();

  function validate(data) {
    if (!data.name) {
      return 'Debe proporcionar un nombre para el local.';
    }
  }

  return <FormScreen
      showCommerceName={true}
      service={useStore()}
      createTitle="Agregar local"
      updateTitle="Modificar local"
      loadingError="No se pudo cargar el local."
      onSuccessNavigate={['Drawer', { screen: 'StoresList'}]}
      defaultData={{ isEnabled: true }}
      fields={[
        'isEnabled',
        'name',
        'description',
      ]}
      validate={validate}
      additionalData={{
        commerceUuid
      }}
    />;
}