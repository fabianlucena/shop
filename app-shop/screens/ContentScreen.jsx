import { NavigationContainer } from '@react-navigation/native';
import { createDrawerNavigator } from '@react-navigation/drawer';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

import HomeScreen from './HomeScreen';
import LogoutScreen from './LogoutScreen';
import ChangePasswordScreen from './ChangePasswordScreen';
import BusinessListScreen from './BusinessListScreen';
import BusinessFormScreen from './BusinessFormScreen';
import ProductListScreen from './ProductListScreen';
import ProductFormScreen from './ProductFormScreen';

const Drawer = createDrawerNavigator();
const Stack = createNativeStackNavigator();

function DrawerNavigator() {
  return <Drawer.Navigator>
      <Drawer.Screen name="Home"           component={HomeScreen}           options={{ title: 'Comprar' }}/>
      <Drawer.Screen name="ChangePassword" component={ChangePasswordScreen} options={{ title: 'Cambiar contraseña' }}/>
      <Drawer.Screen name="BusinessList"   component={BusinessListScreen}   options={{ title: 'Negocios' }}/>
      <Drawer.Screen name="ProductList"    component={ProductListScreen}    options={{ title: 'Artículos' }}/>
      <Drawer.Screen name="Logout"         component={LogoutScreen}         options={{ title: 'Salir' }}/>
    </Drawer.Navigator>;
}

export default function ContentScreen() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen name="Drawer"       component={DrawerNavigator}    options={{ headerShown: false }} />
        <Stack.Screen name="BusinessForm" component={BusinessFormScreen} options={{ title: 'Negocio' }}/>
        <Stack.Screen name="ProductForm"  component={ProductFormScreen}  options={{ title: 'Artículo' }}/>
      </Stack.Navigator>
    </NavigationContainer>
  );
}