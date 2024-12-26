import { NavigationContainer } from '@react-navigation/native';
//import { createDrawerNavigator } from '@react-navigation/drawer';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

import HomeScreen from './HomeScreen';
import LogoutScreen from './LogoutScreen';
import ChangePasswordScreen from './ChangePasswordScreen';
import SellerScreen from './SellerScreen';

const Stack = createNativeStackNavigator();
//const Drawer = createDrawerNavigator();

export default function MainScreen() {
/*
        <Drawer.Navigator>
        <Stack.Screen name="Home"           component={HomeScreen}           options={{ title: 'Principal' }}/>
        <Stack.Screen name="ChangePassword" component={ChangePasswordScreen} options={{ title: 'Cambiar contraseña' }}/>
      </Drawer.Navigator>
*/

  return (
    <NavigationContainer>
      <Stack.Navigator >
        <Stack.Screen name="Home"           component={HomeScreen}           options={{ title: 'Principal' }}/>
        <Stack.Screen name="ChangePassword" component={ChangePasswordScreen} options={{ title: 'Cambiar contraseña' }}/>
        <Stack.Screen name="Seller"         component={SellerScreen}         options={{ title: 'Vendedor' }}/>
        <Stack.Screen name="Logout"         component={LogoutScreen}         options={{ title: 'Salir' }}/>
      </Stack.Navigator>
    </NavigationContainer>
  );
}
