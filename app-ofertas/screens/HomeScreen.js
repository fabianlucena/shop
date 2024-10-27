import { View, Text } from 'react-native';
import Background from '../components/Background';
import styles from '../libs/styles';

export default function HomeScreen() {
  return (
    <Background>
      <View style={styles.container}>
        <Text>
          Pantalla principal
        </Text>
      </View>
    </Background>
  );
}