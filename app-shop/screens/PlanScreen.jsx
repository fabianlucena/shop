import { useEffect, useState } from 'react';
import { ScrollView } from 'react-native';
import { useNavigation } from '@react-navigation/native';

import Screen from '../components/Screen';
import LabelData from '../components/LabelData';
import Button from '../components/Button';
import UsedLeftPieChart from '../components/UsedLeftPieChart';
import usePlan from '../services/usePlan';
import { toFixed } from '../libs/format';

export default function PlanScreen() {
  const [loading, setLoading] = useState(false);
  const [plan, setPlan] = useState({});
  const planService = usePlan();
  const navigation = useNavigation();

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
            label="Imágenes de comercios:"
            description="Cantidad máxima de imágenes de comercios."
            data={<UsedLeftPieChart
              used={plan.used.totalCommercesImagesCount}
              total={plan.available.maxTotalCommercesImages}
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
            label="Productos:"
            description="Cantidad máxima de productos:"
            data={<UsedLeftPieChart
              used={plan.used.totalItemsCount}
              total={plan.available.maxTotalItems}
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
            label="Capacidad para imágenes de productos:"
            description="Capacidad máxima para imágenes de productos."
            data={<UsedLeftPieChart
              used={plan.used.itemsImagesAggregatedSize}
              total={plan.available.maxItemsImagesAggregatedSize}
              numberFormat={n => toFixed(n / 1000000, 2) + ' MB'}
            />}
          />
        </>}

        <Button
          onPress={() => navigation.navigate('PlanDetail')}
        >
          Detalle
        </Button>
      </ScrollView>
    </Screen>;
}
