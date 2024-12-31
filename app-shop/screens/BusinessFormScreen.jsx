import FormScreen from '../components/FormScreen';
import useBusiness from '../services/useBusiness';
import { useSession } from '../contexts/Session';

export default function BusinessFormScreen() {
  const { loadBussiness } = useSession();

  function validate(data) {
    if (!data.name) {
      return 'Debe proporcionar un nombre para el negocio.';
    }
    
    if (!data.description) {
      return 'Debe proporcionar una descripción para el negocio.';
    }
  }

  return <FormScreen
      service={useBusiness()}
      createTitle="Agregar negocio"
      updateTitle="Modificar negocio"
      loadingError="No se pudo cargar el negocio."
      onSuccess={loadBussiness}
      onSuccessNavigate={['Drawer', { screen: 'BusinessesList'}]}
      fields={[
        'isEnabled',
        'name',
        'description',
      ]}
      validate={validate}
    />;
}