import { useEffect, useState } from 'react';
import { ScrollView, View } from 'react-native';

import Screen from '../components/Screen';
import LabelData from '../components/LabelData';
import UsedLeftPieChart from '../components/UsedLeftPieChart';
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
            data={<UsedLeftPieChart
              used={plan.used.totalCommercesCount}
              total={plan.available.maxTotalCommerces}
            />}
          />

          <LabelData
            label="Comercios habilitados:"
            description="Cantidad máxima de comercios habilitados."
            data={<UsedLeftPieChart
              used={plan.used.enabledCommercesCount}
              total={plan.available.maxEnabledCommerces}
            />}
          />

          <LabelData
            label="Tiendas:"
            description="Cantidad máxima de tiendas."
            data={<UsedLeftPieChart
              used={plan.used.totalStoresCount}
              total={plan.available.maxTotalStores}
            />}
          />

          <LabelData
            label="Tiendas habilitadas:"
            description="Cantidad máxima de tiendas habilitadas."
            data={<UsedLeftPieChart
              used={plan.used.enabledStoresCount}
              total={plan.available.maxEnabledStores}
            />}
          />

          <LabelData
            label="Productos:"
            description="Cantidad máxima de productos:"
            data={<UsedLeftPieChart
              used={plan.used.totalItemsCount}
              total={plan.available.maxTotalItems}
            />}
          />

          <LabelData
            label="Productos habilitados:"
            description="Cantidad máxima de productos habilitados."
            data={<UsedLeftPieChart
              used={plan.used.enabledItemsCount}
              total={plan.available.maxEnabledItems}
            />}
          />

          <LabelData
            label="Imágenes de productos:"
            description="Cantidad máxima de imágenes de productos."
            data={<UsedLeftPieChart
              used={plan.used.totalItemsImagesCount}
              total={plan.available.maxTotalItemsImages}
            />}
          />

          <LabelData
            label="Imágenes de productos habilitados:"
            description="Cantidad máxima de imágenes de productos habilitados."
            data={<UsedLeftPieChart
              used={plan.used.enabledItemsImagesCount}
              total={plan.available.maxEnabledItemsImages}
            />}
          />

          <LabelData
            label="Tamaño de imágenes de productos:"
            description="Tamaño máximo para imágenes de productos."
            data={toFixed(plan.available.maxItemImageSize / 1000000, 2) + ' MB'}
          />

          <LabelData
            label="Capacidad para imágenes de productos:"
            description="Capacidad máxima para imágenes de productos."
            data={<UsedLeftPieChart
              used={plan.used.itemsImagesAggregatedSize}
              total={plan.available.maxItemsImagesAggregatedSize}
              numberFormat={n => toFixed(n / 1000000, 2) + ' MB'}
            />}
          />

          <LabelData
            label="Capacidad para imágenes de productos habilitados:"
            description="Capacidad máxima para imágenes de productos habilitados."
            data={<UsedLeftPieChart
              used={plan.used.enabledItemsImagesAggregatedSize}
              total={plan.available.maxEnabledItemsImagesAggregatedSize}
              numberFormat={n => toFixed(n / 1000000, 2) + ' MB'}
            />}
          />

          <LabelData
            label="Tamaño de imágenes de comercios:"
            description="Tamaño máximo para imágenes de comercios."
            data={toFixed(plan.available.maxCommerceImageSize / 1000000, 2) + ' MB'}
          />

          <LabelData
            label="Capacidad para imágenes de comercios:"
            description="Capacidad máxima para imágenes de comercios."
            data={<UsedLeftPieChart
              used={plan.used.commercesImagesAggregatedSize}
              total={plan.available.maxCommercesImagesAggregatedSize}
              numberFormat={n => toFixed(n / 1000000, 2) + ' MB'}
            />}
          />

          <LabelData
            label="Capacidad para imágenes de comercios habilitados:"
            description="Capacidad máxima para imágenes de comercios habilitados."
            data={<UsedLeftPieChart
              used={plan.used.enabledCommercesImagesAggregatedSize}
              total={plan.available.maxEnabledCommercesImagesAggregatedSize}
              numberFormat={n => toFixed(n / 1000000, 2) + ' MB'}
            />}
          />
        </>}
      </ScrollView>
    </Screen>;
}
