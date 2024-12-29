import '@expo/metro-runtime';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { SessionProvider } from './contexts/Session';
import AppScreen from './screens/AppScreen';

export default function App() {
  return (
    <SessionProvider>
      <SafeAreaProvider >
        <AppScreen />
      </SafeAreaProvider>
    </SessionProvider>
  );
}
