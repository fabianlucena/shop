import FormScreen from '../components/FormScreen';
import useBusiness from '../services/useBusiness';

export default function BusinessFormScreen() {
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
      onSuccessNavigate={['Drawer', { screen: 'BusinessesList'}]}
      fields={[
        'isEnabled',
        'name',
        'description',
      ]}
      validate={validate}
    />;
}