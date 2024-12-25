import { View, ActivityIndicator } from 'react-native';
import styles from '../libs/styles';

export default function Busy({busy, children}) {
  return (
    <View style={styles.busy}>
      {children}
      { busy? (
          <View style={{...styles.busyIndicatorContainer}}>
            <ActivityIndicator size="" color="#0060E0" style={{...styles.busyIndicator}} />
          </View>
        ): null
      }
    </View >
  );
}
