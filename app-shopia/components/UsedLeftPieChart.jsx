import { View, Text } from 'react-native';
import { PieChart } from 'react-native-chart-kit';

export default function UsedLeftPieChart({
  used,
  left,
  total,
  label,
  numberFormat = (n) => n,
}) {
  return <View style={{flexDirection: 'row', alignItems: 'center'}}>
      <PieChart
        data={[
          { color: "blue", value: used },
          { color: "lightblue", value: left ?? total - used },
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
      <Text>{label ?? numberFormat(used)  + ' de ' + numberFormat(total ?? left + used) + ' (' + numberFormat(left ?? total - used) + ')'}</Text>
    </View>;
}