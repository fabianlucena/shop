import FormScreen from '../components/FormScreen';
import useStore from '../services/useStore';

export default function StoreFormScreen() {
  function validate(data) {
    if (!data.name) {
      return 'Debe proporcionar un nombre para el local.';
    }
  }

  return <FormScreen
      service={useStore()}
      createTitle="Agregar local"
      updateTitle="Modificar local"
      loadingError="No se pudo cargar el local."
      onSuccessNavigate={['Drawer', { screen: 'StoresList'}]}
      fields={[
        'isEnabled',
        'name',
        'description',
      ]}
      validate={validate}
    />;
}