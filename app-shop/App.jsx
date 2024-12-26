import '@expo/metro-runtime';
import { SessionProvider } from './contexts/Session';
import Shop from './Shop';

export default function App() {
  return (
    <SessionProvider>
      <Shop />
    </SessionProvider>
  );
}
