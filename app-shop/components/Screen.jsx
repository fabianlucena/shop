import { View, Text } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';

import { useSession } from '../contexts/Session';

import Background from './Background';
import Busy from './Busy';
import Header from './Header';
import styles from '../libs/styles';

export default function Screeen({
  children,
  busy,
  header,
  showBusinessName = false,
}) {
  const { businessName } = useSession();
  const insets = useSafeAreaInsets();

  return <Background>
      <Busy
        busy={busy}
      >
        <View style={[
          styles.screen,
          {
            paddingTop: insets.top,
            paddingBottom: insets.bottom,
            paddingLeft: insets.left,
            paddingRight: insets.right,
          }
        ]}>
          {showBusinessName && businessName && <Text style={{...styles.text, ...styles.header, ...styles.currentBussiness}}>Administrando: {businessName}</Text> || null}
          {header && <Header>{header}</Header> || null}
          {children}
        </View>
      </Busy>
    </Background>;
}