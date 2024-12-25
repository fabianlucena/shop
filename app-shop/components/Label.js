import styles from '../libs/styles';
import { Text } from 'react-native';

export default function Label({children, style}) {
  return (<Text style={{...styles.label, ...styles.text, ...style}}>{children}</Text>);
}