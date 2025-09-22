import '@expo/metro-runtime';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { SessionProvider } from './components/Session';
import AppScreen from './screens/AppScreen';
import * as ImagePicker from 'expo-image-picker';
import { useEffect } from 'react';

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
  useEffect(() => {
    (async () => {
      let result = await ImagePicker.requestMediaLibraryPermissionsAsync();
      if (result.status !== "granted") {
        alert("Se necesita permiso para acceder a la galería.");
      }

      result = await ImagePicker.requestCameraPermissionsAsync();
      if (result.status !== "granted") {
        alert("Se necesita permiso para acceder a la cámara.");
      }
    })();
  }, []);

  return (
    <SessionProvider>
      <SafeAreaProvider >
        <AppScreen />
      </SafeAreaProvider>
    </SessionProvider>
  );
}
