import FormScreen from '../components/FormScreen';
import useCommerce from '../services/useCommerce';
import { useSession } from '../contexts/Session';

function validate(data) {
  if (!data.name) {
    return 'Debe proporcionar un nombre para el comercio.';
  }
  
  if (!data.description) {
    return 'Debe proporcionar una descripción para el comercio.';
  }
}

export default function CommerceFormScreen() {
  const { loadCommerce } = useSession();

  return <FormScreen
      service={useCommerce()}
      createTitle="Agregar comercio"
      updateTitle="Modificar comercio"
      loadingError="No se pudo cargar el comercio."
      onSuccess={loadCommerce}
      onSuccessNavigate={['Drawer', { screen: 'CommercesList'}]}
      fields={[
        'isEnabled',
        'name',
        'description',
      ]}
      validate={validate}
    />;
}