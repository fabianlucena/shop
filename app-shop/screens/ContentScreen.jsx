import { NavigationContainer } from '@react-navigation/native';
import { createDrawerNavigator } from '@react-navigation/drawer';

import HomeScreen from './HomeScreen';
import LogoutScreen from './LogoutScreen';
import ChangePasswordScreen from './ChangePasswordScreen';
import SellerScreen from './SellerScreen';

const Drawer = createDrawerNavigator();

export default function ContentScreen() {
  return (
    <NavigationContainer>
      <Drawer.Navigator>
        <Drawer.Screen name="Home"           component={HomeScreen}           options={{ title: 'Principal', headerBackgroundColor: 'red' }}/>
        <Drawer.Screen name="ChangePassword" component={ChangePasswordScreen} options={{ title: 'Cambiar contraseña' }}/>
        <Drawer.Screen name="Seller"         component={SellerScreen}         options={{ title: 'Vendedor' }}/>
        <Drawer.Screen name="Logout"         component={LogoutScreen}         options={{ title: 'Salir' }}/>
      </Drawer.Navigator>
    </NavigationContainer>
  );
}
