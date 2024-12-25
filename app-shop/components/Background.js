import { View } from 'react-native';
import styles from '../libs/styles';

export default function Background({children}) {
  return (
    <View style={styles.background}>
      {children}
    </View >
  );
}
