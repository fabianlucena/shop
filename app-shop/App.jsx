import '@expo/metro-runtime';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { SessionProvider } from './contexts/Session';
import AppScreen from './screens/AppScreen';

if (typeof window !== 'undefined') {
  const originalWarn = console.warn;
  console.warn = (...args) => {
    if (typeof args[0] === 'string' && args[0].includes('props.pointerEvents is deprecated')) {
      return;
    }
    originalWarn(...args);
  };
}

export default function App() {
  return (
    <SessionProvider>
      <SafeAreaProvider >
        <AppScreen />
      </SafeAreaProvider>
    </SessionProvider>
  );
}
