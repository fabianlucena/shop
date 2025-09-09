import { View, Text } from 'react-native';
import { PieChart } from 'react-native-chart-kit';

export default function UsedAvailablePieChart({
  used,
  available,
  total,
  label,
}) {
  return <View style={{flexDirection: 'row', alignItems: 'center'}}>
      <PieChart
        data={[
          { color: "blue", value: used },
          { color: "lightblue", value: available ?? total - used },
        ]}
        width={35}
        height={25}
        chartConfig={{
          color: () => `rgba(0, 0, 0, 1)`
        }}
        hasLegend={false}
        accessor="value"
        backgroundColor="transparent"
        paddingLeft="3"
      />
      <Text>{label ?? used  + ' / ' + total ?? available + used}</Text>
    </View>;
}