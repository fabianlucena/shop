import { useEffect, useState } from 'react';

import Screen from '../components/Screen';
import LabelData from '../components/LabelData';

import usePlan from '../services/usePlan';

export default function PlanScreen() {
  const [loading, setLoading] = useState(false);
  const [plan, setPlan] = useState({});
  const planService = usePlan();

  function loadPlan() {
    setLoading(true);
    planService.get()
      .then(setPlan)
      .finally(() => setLoading(false));
  }

  useEffect(() => {
    loadPlan();
  }, []);

  return <Screen
      header='Plan'
      busy={loading}
      showCommerceName
      style={{
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <LabelData
        label="Nombre:"
        data={plan?.available?.name}
        style={{
          backgroundColor: '#ddd',
        }}
      />
      <LabelData
        label="Descripción:"
        data={plan?.available?.description}
      />
      
      <LabelData
        label="Cantidad máxima de comercios:"
        data={plan?.available?.maxTotalCommerces}
      />

      <LabelData
        label="Cantidad máxima de comercios habilitados:"
        data={plan?.available?.maxEnabledCommerces}
      />

      <LabelData
        label="Cantidad máxima de tiendas:"
        data={plan?.available?.maxTotalStores}
      />

      <LabelData
        label="Cantidad máxima de tiendas habilitadas:"
        data={plan?.available?.maxEnabledStores}
      />

      <LabelData
        label="Cantidad máxima de productos:"
        data={plan?.available?.maxTotalItems}
      />

      <LabelData
        label="Cantidad máxima de productos habilitados:"
        data={plan?.available?.maxEnabledItems}
      />

      <LabelData
        label="Cantidad máxima de imágenes de productos:"
        data={plan?.available?.maxTotalItemsImages}
      />

      <LabelData
        label="Cantidad máxima de imágenes de productos habilitadas:"
        data={plan?.available?.maxEnabledItemsImages}
      />

      <LabelData
        label="Capacidad máxima para imágenes de productos:"
        data={Math.round(plan?.available?.maxAggregattedSizeItemsImages / 1000000) + ' MB'}
      />

      <LabelData
        label="Capacidad máxima para imágenes de productos habilitados:"
        data={Math.round(plan?.available?.maxEnabledAggregattedSizeItemsImages / 1000000) + ' MB'}
      />
    </Screen>;
}
