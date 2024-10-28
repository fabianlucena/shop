import { View, Text } from 'react-native';
import Background from '../components/Background';
import Button from '../components/Button';
import styles from '../libs/styles';
import FancyText from '../components/FancyText';

export default function HomeScreen({ navigation }) {
  return (
    <Background>
      <View style={styles.container}>
        <FancyText>
          Pantalla principal
        </FancyText>
        <Button onPress={() => navigation.navigate('ChangePassword')} >Cambiar contraseña</Button>
        <Button onPress={() => navigation.navigate('Logout')} >Salir</Button>
      </View>
    </Background>
  );
}