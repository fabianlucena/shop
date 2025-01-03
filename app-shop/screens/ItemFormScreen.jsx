import FormScreen from '../components/FormScreen';
import useItem from '../services/useItem';

function validate(data, fields) {
  const minAgeField = fields.find(f => f.name === 'minAge');
  if (minAgeField)
    minAgeField.visible = !!data.isPresent;

  const maxAgeField = fields.find(f => f.name === 'maxAge');
  if (maxAgeField)
    maxAgeField.visible = !!data.isPresent;

  if (data.minAge && data.maxAge && parseFloat(data.minAge) > parseFloat(data.maxAge))
    return 'La edad mínima no puede ser mayor a la edad máxima.';
  
  return true;
}

export default function ItemFormScreen() {
  const categories = [
    { label: 'Opción 1', value: 'opcion1' },
    { label: 'Opción 2', value: 'opcion2' },
    { label: 'Opción 3', value: 'opcion3' },
  ];

  return (
    <FormScreen
      showBusinessName={true}
      service={useItem()}
      createTitle="Agregar artículo"
      updateTitle="Modificar artículo"
      loadingError="No se pudo cargar el artículo."
      onSuccessNavigate={['Drawer', { screen: 'ItemsList'}]}
      fields={[
        'isEnabled',
        'name',
        'description',
        { name: 'category',  type: 'select',   label: 'Rubro', options: categories },
        { name: 'store',     type: 'select',   label: 'Local', options: categories },
        { name: 'price',     type: 'currency', label: 'Precio' },
        { name: 'stock',     type: 'number',   label: 'Disponibilidad' },
        { name: 'isPresent', type: 'switch',   label: 'Apto para regalar' },
        { name: 'minAge',    type: 'number',   label: 'Edad mínima' },
        { name: 'maxAge',    type: 'number',   label: 'Edad máxima' },
      ]}
      validate={validate}
    />
  );
}