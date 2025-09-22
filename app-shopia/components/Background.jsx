import { View } from 'react-native';
import styles from '../libs/styles';

export default function Background({children, style}) {
  return (
    <View
      style={{
        ...styles.background,
        ...style,
        width: '100%',
      }}
    >
      {children}
    </View >
  );
}
