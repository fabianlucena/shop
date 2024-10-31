import { View } from 'react-native';
import Background from './Background';
import FancyText from './FancyText';
import Button from './Button';
import styles from '../libs/styles';

export default function RegisterScreen({ children, onPress, onPressMessage = 'Continuar' }) {
  return (
    <Background>
      <View style={styles.container}>
        <FancyText>{children}</FancyText>
        <Button
          onPress={onPress}
        >
          {onPressMessage}
        </Button>
      </View>
    </Background>
  );    
}