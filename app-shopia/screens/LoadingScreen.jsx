import { ActivityIndicator } from 'react-native';

import styles from '../libs/styles';

import Screen from '../components/Screen';
import Header from '../components/Header';

export default function LoadingScreen({ message = 'Cargando'}) {
  return <Screen>
      {message && <Header>{message}</Header>}
      <ActivityIndicator size="large" color={styles.loader.color} style={{...styles.loader }} />
    </Screen>;
 }
