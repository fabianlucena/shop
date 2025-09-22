import { View, Text } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';

import { useSession } from './Session';

import Messages from './Messages';
import Background from './Background';
import Busy from './Busy';
import Header from './Header';
import styles from '../libs/styles';

export default function Screeen({
  children,
  busy,
  header,
  showCommerceName = false,
}) {
  const { commerceName } = useSession();
  const insets = useSafeAreaInsets();

  return <Background>
      <Busy
        busy={busy}
      >
        <View style={[
          styles.screen,
          {
            paddingTop: 0,
            paddingBottom: insets.bottom,
            paddingLeft: 0,
            paddingRight: 0,
          }
        ]}>
          {showCommerceName && commerceName && <Text style={{...styles.text, ...styles.header, ...styles.currentCommerce}}>Administrando: {commerceName}</Text> || null}
          {header && <Header>{header}</Header> || null}
          <Messages />
          {children}
        </View>
      </Busy>
    </Background>;
}