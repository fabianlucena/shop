import { View } from 'react-native';
import Background from '../components/Background';
import styles from '../libs/styles';
import FancyText from '../components/FancyText';
import Menu from '../components/Menu';

export default function HomeScreen() {
  return (
    <Background>
      <View style={styles.container}>
        <FancyText>
          Pantalla principal
        </FancyText>
        <Menu />
      </View>
    </Background>
  );
}