import { View } from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';

import { useSession } from '../contexts/Session';

import Background from './Background';
import Busy from './Busy';
import Header from './Header';
import FancyText from './FancyText';
import styles from '../libs/styles';

export default function Screeen({ children, busy, header }) {
  const { business } = useSession();
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
          {business && <FancyText>Negocio: {business}</FancyText> || null}
          {header && <Header>{header}</Header> || null}
          {children}
        </View>
      </Busy>
    </Background>
  );
}