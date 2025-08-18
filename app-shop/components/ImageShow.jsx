import { useState, useEffect } from 'react';
import { Image } from 'react-native';
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

  return <Image
      source={{ uri }}
      style={style}
      resizeMode="cover"
    />;
}