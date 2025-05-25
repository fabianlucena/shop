import { NavigationContainer } from '@react-navigation/native';
import { createDrawerNavigator, DrawerContentScrollView, DrawerItemList } from '@react-navigation/drawer';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

import { useSession } from '../contexts/Session';

import ButtonIconAdd from '../components/ButtonIconAdd';

import ExploreScreen from './ExploreScreen';
import LogoutScreen from './LogoutScreen';
import ChangePasswordScreen from './ChangePasswordScreen';
import CommercesListScreen from './CommercesListScreen';
import CommerceFormScreen from './CommerceFormScreen';
import StoresListScreen from './StoresListScreen';
import StoreFormScreen from './StoreFormScreen';
import ItemListScreen from './ItemListScreen';
import ItemFormScreen from './ItemFormScreen';
import SelectField from '../components/SelectField';

const Drawer = createDrawerNavigator();
const Stack = createNativeStackNavigator();

function DrawerNavigator() {
  const { commerces, commerceUuid, setCommerceUuid } = useSession();

  return <Drawer.Navigator
      drawerContent={props => <DrawerContentScrollView {...props}>
          <SelectField
            key="selector"
            options={commerces.map(i => ({value: i.uuid, label: i.name}))}
            placeholder="--- Sin comercio ---"
            value={commerceUuid || ''}
            onChangeValue={setCommerceUuid}
          />
          <DrawerItemList {...props} />
        </DrawerContentScrollView>}
    >
      <Drawer.Screen name="Explore"        component={ExploreScreen}        options={{ title: 'Explorar' }} />
      <Drawer.Screen name="CommercesList"  component={CommercesListScreen}  options={{ title: 'Mis comercios', headerRight: () => <ButtonIconAdd style={{margin: 10}} size="big" navigate="CommerceForm" /> }}/>
      <Drawer.Screen name="StoresList"     component={StoresListScreen}     options={{ title: 'Mis locales',   headerRight: () => <ButtonIconAdd style={{margin: 10}} size="big" navigate="StoreForm"    /> }}/>
      <Drawer.Screen name="ItemsList"      component={ItemListScreen}       options={{ title: 'Mis artículos', headerRight: () => <ButtonIconAdd style={{margin: 10}} size="big" navigate="ItemForm"     /> }}/>
      <Drawer.Screen name="ChangePassword" component={ChangePasswordScreen} options={{ title: 'Cambiar contraseña' }}/>
      <Drawer.Screen name="Logout"         component={LogoutScreen}         options={{ title: 'Salir' }}/>
    </Drawer.Navigator>;
}

export default function ContentScreen() {
  return <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen name="Drawer"       component={DrawerNavigator}    options={{ headerShown: false }} />
        <Stack.Screen name="CommerceForm" component={CommerceFormScreen} options={{ title: 'Comercio' }}/>
        <Stack.Screen name="StoreForm"    component={StoreFormScreen}    options={{ title: 'Local' }}/>
        <Stack.Screen name="ItemForm"     component={ItemFormScreen}     options={{ title: 'Artículo' }}/>
      </Stack.Navigator>
    </NavigationContainer>;
}
