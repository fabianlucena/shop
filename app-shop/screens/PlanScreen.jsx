import { useEffect, useState } from 'react';
import { ScrollView, View } from 'react-native';

import Screen from '../components/Screen';
import LabelData from '../components/LabelData';
import UsedAvailablePieChart from '../components/UsedAvailablePieChart';
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
            data={<UsedAvailablePieChart
              used={plan.used.totalCommercesCount}
              total={plan.available.maxTotalCommerces}
            />}
          />

          <LabelData
            label="Comercios habilitados:"
            description="Cantidad máxima de comercios habilitados."
            data={<UsedAvailablePieChart
              used={plan.used.enabledCommercesCount}
              total={plan.available.maxEnabledCommerces}
            />}
          />

          <LabelData
            label="Tiendas:"
            description="Cantidad máxima de tiendas."
            data={<UsedAvailablePieChart
              used={plan.used.totalStoresCount}
              total={plan.available.maxTotalStores}
            />}
          />

          <LabelData
            label="Tiendas habilitadas:"
            description="Cantidad máxima de tiendas habilitadas."
            data={<UsedAvailablePieChart
              used={plan.used.enabledStoresCount}
              total={plan.available.maxEnabledStores}
            />}
          />

          <LabelData
            label="Productos:"
            description="Cantidad máxima de productos:"
            data={<UsedAvailablePieChart
              used={plan.used.totalItemsCount}
              total={plan.available.maxTotalItems}
            />}
          />

          <LabelData
            label="Productos habilitados:"
            description="Cantidad máxima de productos habilitados."
            data={<UsedAvailablePieChart
              used={plan.used.enabledItemsCount}
              total={plan.available.maxEnabledItems}
            />}
          />

          <LabelData
            label="Imágenes de productos:"
            description="Cantidad máxima de imágenes de productos."
            data={<UsedAvailablePieChart
              used={plan.used.totalItemsImagesCount}
              total={plan.available.maxTotalItemsImages}
            />}
          />

          <LabelData
            label="Imágenes de productos habilitados:"
            description="Cantidad máxima de imágenes de productos habilitados."
            data={<UsedAvailablePieChart
              used={plan.used.enabledItemsImagesCount}
              total={plan.available.maxEnabledItemsImages}
            />}
          />

          <LabelData
            label="Capacidad para imágenes de productos:"
            description="Capacidad máxima para imágenes de productos."
            data={<UsedAvailablePieChart
              used={plan.used.aggregattedSizeItemsImages}
              total={plan.available.maxAggregattedSizeItemsImages}
              label={
                toFixed(plan.used.aggregattedSizeItemsImages / 1000000 || 0, 2)
                + ' / ' +
                toFixed(plan.available.maxAggregattedSizeItemsImages / 1000000, 2) + ' MB'
              }
            />}
          />

          <LabelData
            label="Capacidad para imágenes de productos habilitados:"
            description="Capacidad máxima para imágenes de productos habilitados."
            data={<UsedAvailablePieChart
              used={plan.used.enabledAggregattedSizeItemsImages}
              total={plan.available.maxEnabledAggregattedSizeItemsImages}
              label={
                toFixed(plan.used.enabledAggregattedSizeItemsImages / 1000000 || 0, 2)
                + ' / ' +
                toFixed(plan.available.maxEnabledAggregattedSizeItemsImages / 1000000, 2) + ' MB'
              }
            />}
          />
        </>}
      </ScrollView>
    </Screen>;
}
