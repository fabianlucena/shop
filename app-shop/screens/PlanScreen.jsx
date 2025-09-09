import { useEffect, useState } from 'react';
import { ScrollView } from 'react-native';

import Screen from '../components/Screen';
import LabelData from '../components/LabelData';

import usePlan from '../services/usePlan';

import { toFixed } from '../libs/format';

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
    >
      <ScrollView>
        {plan.available && <>
          <LabelData
            label="Nombre:"
            data={plan.available.name}
          />
          {plan.available.description && <LabelData
            label="Descripción:"
            data={plan.available.description}
          />}
          <LabelData
            label="Comercios:"
            description="Cantidad máxima de comercios."
            data={plan.used.totalCommercesCount  + ' / ' + plan.available.maxTotalCommerces}
          />

          <LabelData
            label="Comercios habilitados:"
            description="Cantidad máxima de comercios habilitados."
            data={plan.used.enabledCommercesCount  + ' / ' + plan.available.maxEnabledCommerces}
          />

          <LabelData
            label="Tiendas:"
            description="Cantidad máxima de tiendas."
            data={plan.used.totalStoresCount  + ' / ' + plan.available.maxTotalStores}
          />

          <LabelData
            label="Tiendas habilitadas:"
            description="Cantidad máxima de tiendas habilitadas."
            data={plan.used.enabledStoresCount  + ' / ' + plan.available.maxEnabledStores}
          />

          <LabelData
            label="Productos:"
            description="Cantidad máxima de productos:"
            data={plan.used.totalItemsCount  + ' / ' + plan.available.maxTotalItems}
          />

          <LabelData
            label="Productos habilitados:"
            description="Cantidad máxima de productos habilitados."
            data={plan.used.enabledItemsCount  + ' / ' + plan.available.maxEnabledItems}
          />

          <LabelData
            label="Imágenes de productos:"
            description="Cantidad máxima de imágenes de productos."
            data={plan.used.totalItemsImagesCount  + ' / ' + plan.available.maxTotalItemsImages}
          />

          <LabelData
            label="Imágenes de productos habilitados:"
            description="Cantidad máxima de imágenes de productos habilitados."
            data={plan.used.enabledItemsImagesCount  + ' / ' + plan.available.maxEnabledItemsImages}
          />

          <LabelData
            label="Capacidad para imágenes de productos:"
            description="Capacidad máxima para imágenes de productos."
            
            data={
              toFixed(plan.used.aggregattedSizeItemsImages / 1000000 || 0, 2)
              + ' / ' +
              toFixed(plan.available.maxAggregattedSizeItemsImages / 1000000, 2) + ' MB'
            }
          />

          <LabelData
            label="Capacidad para imágenes de productos habilitados:"
            description="Capacidad máxima para imágenes de productos habilitados."
            data={
              toFixed(plan.used.enabledAggregattedSizeItemsImages / 1000000 || 0, 2)
              + ' / ' +
              toFixed(plan.available.maxEnabledAggregattedSizeItemsImages / 1000000, 2) + ' MB'
            }
          />
        </>}
      </ScrollView>
    </Screen>;
}
