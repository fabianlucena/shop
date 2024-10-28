import { View, Text } from 'react-native';
import Background from '../components/Background';
import Button from '../components/Button';
import styles from '../libs/styles';

export default function HomeScreen({ navigation }) {
  return (
    <Background>
      <View style={styles.container}>
        <Text>
          Pantalla principal
        </Text>
        <Button onPress={() => navigation.navigate('ChangePassword')} >Cambiar contraseña</Button>
        <Button onPress={() => navigation.navigate('Logout')} >Salir</Button>
      </View>
    </Background>
  );
}