import styles from '../libs/styles';
import { Text } from 'react-native';

export default function Label({children, style}) {
  return <Text style={{...styles.text, ...styles.label, ...style}}>{children}</Text>;
}