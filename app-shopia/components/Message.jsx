import styles from '../libs/styles';
import { View, Text, Pressable } from 'react-native';
import useSession from './Session';

export default function Message({
  children,
  variant = 'info',
  style
}) {
  if (!children)
    return null;

  return <View
      style={styles.messageContainer}
    >
      <Text
        style={{
          ...styles.text,
          ...styles.message,
          ...(variant === 'error' ? styles.error : {}),
          ...style
        }}
      >
        {children}
      </Text>
    </View>;
}