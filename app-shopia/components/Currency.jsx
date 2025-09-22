import { Text } from 'react-native';
import { formatCurrency } from '../libs/format';

export default function Currency({
  style,
  children,
}) {
  return <Text
      style={style}
    >
      {formatCurrency(children)}
    </Text>;
}