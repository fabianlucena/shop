import { useState, useEffect } from 'react';
import { Image, StyleSheet, View, ActivityIndicator } from 'react-native';
import { Buffer } from 'buffer';

export default function ImageShow({
  style,
  service,
  image,
}) {
  const [contentType, setContentType] = useState('');
  const [uri, setUri] = useState('');

  useEffect(() => {
    setContentType('');
    setUri('');

    service(image)
      .then(res => {
        setContentType(res.headers.get('Content-Type') || 'image/jpeg');
        return res.arrayBuffer();
      })
      .then(arrayBuffer => {
        const base64 = Buffer.from(arrayBuffer).toString('base64');
        setUri(`data:${contentType};base64,${base64}`);
      });
  }, [service, image]);

  console.log(!!uri);

  return <View style={styles.container}>
      {uri && <Image
        source={{ uri }}
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
