import styles from '../libs/styles';
import { View, Text } from 'react-native';

export default function Message({children, style}) {
  return <View
      style={styles.messageContainer}
    >
      <Text style={{...styles.text, ...styles.message, ...style}}>{children}</Text>
    </View>;
}