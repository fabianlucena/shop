import { View } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';

import Background from './Background';
import Busy from './Busy';
import Header from './Header';
import styles from '../libs/styles';

export default function Screeen({ children, busy, header }) {
  const insets = useSafeAreaInsets();

  return (
    <Background>
      <Busy
        busy={busy}
      >
        <View style={[
          styles.container,
          {
            paddingTop: insets.top,
            paddingBottom: insets.bottom,
            paddingLeft: insets.left,
            paddingRight: insets.right,
          }
        ]}>
          {header && <Header>
            {header}
          </Header>}
          {children}
        </View>
      </Busy>
    </Background>
  );
}