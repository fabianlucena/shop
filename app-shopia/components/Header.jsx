import styles from '../libs/styles';
import { Text } from 'react-native';

export default function Header({children, style}) {
  return <Text style={{...styles.text, ...styles.label, ...styles.header, ...style}}>{children}</Text>;
}