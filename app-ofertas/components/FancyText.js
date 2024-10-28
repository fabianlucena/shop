import styles from '../libs/styles';
import { Text } from 'react-native';

export default function FancyText({children, style}) {
  return (<Text style={{...styles.text, ...styles.fancyText, ...style}}>{children}</Text>);
}