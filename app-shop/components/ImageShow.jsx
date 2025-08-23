import { Image, StyleSheet, View, ActivityIndicator } from 'react-native';
import { Api } from '../libs/api';

export default function ImageShow({
  style,
  uri,
  urlBase = Api.urlBase,
}) {
  return <View style={styles.container}>
      {uri && <Image
        source={{ uri: urlBase + uri }}
        style={style}
        resizeMode="cover"
      /> || <ActivityIndicator style={style} size="large" color="#888" />}
    </View>;
}

const styles = StyleSheet.create({
  container: {
    flex: 1, // asegura que el layout se resuelva
    alignItems: 'center',
    justifyContent: 'center',
  },
  image: {
    width: '100%',
    height: '100%',
    borderRadius: 8,
  },
});
