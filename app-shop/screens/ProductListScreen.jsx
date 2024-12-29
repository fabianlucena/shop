import Screen from '../components/Screen';
import Button from '../components/Button';

export default function ProductListScreen({ navigation}) {
  return (
    <Screen>
      <Button onPress={() => navigation.navigate('ProductForm')} >Agregar</Button>
    </Screen>
  );
}