import { View } from 'react-native';
import Background from './Background';
import FancyText from './FancyText';
import Button from './Button';
import styles from '../libs/styles';

export default function ScreenMessage({
  children,
  onPress,
  onPressLabel = 'Continuar'
}) {
  return <Background>
      <View style={styles.container}>
        <FancyText>{children}</FancyText>
        <Button
          onPress={onPress}
        >
          {onPressLabel}
        </Button>
      </View>
    </Background>;
}