import { useState, useEffect } from 'react';
import { Image, StyleSheet, View, ActivityIndicator } from 'react-native';
import { Buffer } from 'buffer';

export default function ImageShow({
  style,
  service,
  uri,
  image,
}) {
  const [localUri, setLocalUri] = useState('');

  useEffect(() => {
    if (uri) {
      setLocalUri(uri);
      return;
    }

    setLocalUri('');
    if (service && image) {
      service(image)
        .then(res => {
          res.arrayBuffer()
            .then(arrayBuffer => {
              const contentType = res.headers.get('Content-Type') || 'image/jpeg';
              const base64 = Buffer.from(arrayBuffer).toString('base64');
              setLocalUri(`data:${contentType};base64,${base64}`);
            });
        });
    }
  }, [uri, service, image]);

  return <View style={styles.container}>
      {localUri && <Image
        source={{ uri: localUri }}
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
