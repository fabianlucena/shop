import { useNavigation } from '@react-navigation/native';
import Button from '../components/Button';
import { useSession } from '../contexts/Session';

export default function Menu() {
  const navigation = useNavigation();
  const { permissions } = useSession();

  console.log(permissions);

  const items = [
    {
      name: 'changePassword',
      permission: 'changePassword',
      title: 'Cambiar contraseña',
      action: () => navigation.navigate('ChangePassword')
    },
    {
      name: 'logout',
      title: 'Salir',
      action: () => navigation.navigate('Logout')
    },
    {
      name: 'seller',
      //permission: 'seller',
      title: 'Vendedor',
      action: () => navigation.navigate('Seller')
    },
  ];

  return items
    .filter(i => !i.permission || permissions.includes(i.permission))
    .map(i => <Button onPress={() => i.action() }>{i.title}</Button>);
}