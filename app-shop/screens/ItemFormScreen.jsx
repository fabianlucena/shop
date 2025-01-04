import FormScreen from '../components/FormScreen';
import useItem from '../services/useItem';
import { useSession } from '../contexts/Session';

function validate(data, fields) {
  const minAgeField = fields.find(f => f.name === 'minAge');
  if (minAgeField)
    minAgeField.visible = !!data.isPresent;

  const maxAgeField = fields.find(f => f.name === 'maxAge');
  if (maxAgeField)
    maxAgeField.visible = !!data.isPresent;
  
  if (!data.name) {
    return 'Debe proporcionar un nombre para el artículo.';
  }
  
  if (!data.description) {
    return 'Debe proporcionar una descripción para el artículo.';
  }
  
  if (!data.categoryUuid) {
    return 'Debe seleccionar un rubro.';
  }

  if (!data.storeUuid) {
    return 'Debe seleccionar un local.';
  }

  if (!data.price) {
    return 'Debe colocar un precio para el artículo.';
  }

  if (!data.stock) {
    return 'Debe colocar la cantidad de artículos disponibles.';
  }
  
  if (data.minAge && data.maxAge && parseFloat(data.minAge) > parseFloat(data.maxAge))
    return 'La edad mínima no puede ser mayor a la edad máxima.';
  
  return true;
}

export default function ItemFormScreen() {
  const { categoriesOptions, storesOptions } = useSession();

  return <FormScreen
      showCommerceName={true}
      service={useItem()}
      createTitle="Agregar artículo"
      updateTitle="Modificar artículo"
      loadingError="No se pudo cargar el artículo."
      onSuccessNavigate={['Drawer', { screen: 'ItemsList'}]}
      defaultData={{ isEnabled: true }}
      fields={[
        'isEnabled',
        'name',
        'description',
        { name: 'categoryUuid', type: 'select',   label: 'Rubro', options: categoriesOptions },
        { name: 'storeUuid',    type: 'select',   label: 'Local', options: storesOptions },
        { name: 'price',        type: 'currency', label: 'Precio' },
        { name: 'stock',        type: 'number',   label: 'Disponibilidad' },
        { name: 'isPresent',    type: 'switch',   label: 'Apto para regalar' },
        { name: 'minAge',       type: 'number',   label: 'Edad mínima' },
        { name: 'maxAge',       type: 'number',   label: 'Edad máxima' },
      ]}
      validate={validate}
    />;
}