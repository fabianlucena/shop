import FormScreen from '../components/FormScreen';
import useCommerce from '../services/useCommerce';
import { useSession } from '../components/Session';

function validate(data) {
  if (!data.name) {
    return 'Debe proporcionar un nombre para el comercio.';
  }
  
  if (!data.description) {
    return 'Debe proporcionar una descripción para el comercio.';
  }
}

export default function CommerceFormScreen() {
  const { loadCommerces } = useSession();

  return <FormScreen
      service={useCommerce()}
      createTitle="Agregar comercio"
      updateTitle="Modificar comercio"
      loadingError="No se pudo cargar el comercio."
      onSuccess={loadCommerces}
      onSuccessNavigate={['Drawer', { screen: 'CommercesList'}]}
      defaultData={{ isEnabled: true }}
      fields={[
        'isEnabled',
        'name',
        'description',
      ]}
      validate={validate}
    />;
}