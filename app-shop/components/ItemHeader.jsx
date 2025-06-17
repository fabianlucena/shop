import styles from '../libs/styles';
import { Text } from 'react-native';

export default function ItemHeader({children, style}) {
  return <Text
      style={{
        ...styles.text,
        ...styles.label,
        ...styles.header,
        ...styles.itemHeader,
        ...style,
      }}
    >
      {children}
    </Text>;
}