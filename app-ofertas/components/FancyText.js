import styles from '../libs/styles';
import { Text } from 'react-native';

export default function FancyText({children, style}) {
  return (<Text style={{...styles.fancyText, ...styles.text, ...style}}>{children}</Text>);
}