import styles from '../libs/styles';
import { Text } from 'react-native';

export default function Hint({children, style}) {
  return (<Text style={{...styles.text, ...styles.hint, ...style}}>{children}</Text>);
}