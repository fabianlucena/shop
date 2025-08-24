import { useState, useEffect } from 'react';
import { Image, View, ActivityIndicator } from 'react-native';
import { Api } from '../libs/api';

export default function ImageShow({
  style,
  uri,
  urlBase = Api.urlBase,
}) {
  const [localStyle, setLocalStyle] = useState({ ...style });

  useEffect(() => {
    const newLocalStyle = {...style};

    if (!newLocalStyle.width) {
      if (newLocalStyle.height && newLocalStyle.aspectRatio) {
        newLocalStyle.width = newLocalStyle.height * newLocalStyle.aspectRatio;
      }
    }

    if (!newLocalStyle.height) {
      if (newLocalStyle.width && newLocalStyle.aspectRatio) {
        newLocalStyle.height = newLocalStyle.width / newLocalStyle.aspectRatio;
      }
    }

    if (newLocalStyle.width)
      newLocalStyle.width = Math.floor(newLocalStyle.width);

    if (newLocalStyle.height)
      newLocalStyle.height = Math.floor(newLocalStyle.height);

    if (JSON.stringify(localStyle) !== JSON.stringify(newLocalStyle)) {
      setLocalStyle(newLocalStyle);
    }
  }, [style]);

  return <View
      style={{
        flex: 1, // asegura que el layout se resuelva
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      {uri && <Image
        source={{ uri: urlBase + uri }}
        style={localStyle}
        resizeMode="cover"
      /> || <ActivityIndicator size="large" color="#888" />}
    </View>;
}