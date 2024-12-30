import styles from '../libs/styles';
import { View, Text } from 'react-native';

export default function Error({children, style}) {
  if (!children)
    return <></>;

  return <View
      style={{...styles.errorContainer, ...styles.messageContainer}}
    >
      <Text style={{...styles.text, ...styles.message, ...styles.error, ...style}}>{children}</Text>
    </View>;
}