import Screen from '../components/Screen';
import Button from '../components/Button';

export default function ItemListScreen({ navigation}) {
  return (
    <Screen>
      <Button onPress={() => navigation.navigate('ItemForm')} >Agregar</Button>
    </Screen>
  );
}