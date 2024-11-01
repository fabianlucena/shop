import styles from '../libs/styles';
import { Text } from 'react-native';

export default function Message({children, style}) {
  return (<Text style={{...styles.text, ...styles.message, ...style}}>{children}</Text>);
}