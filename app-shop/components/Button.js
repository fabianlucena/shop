import { Pressable, ActivityIndicator } from 'react-native';
import styles from '../libs/styles';
import { Text } from 'react-native';
import { useEffect, useState } from 'react';

export default function Button({ children, onPress, disabled, style, styleText }) {
  const [fullStyle, setFullStyle] = useState();
  const [fullTextStyle, setFullTextStyle] = useState();

  useEffect(() => {
    if (disabled) {
      setFullStyle({...styles.button, ...styles.disabledButton, ...style});
      setFullTextStyle({...styles.text, ...styles.textButton, ...styles.disabledTextButton, ...styleText});
    } else {
      setFullStyle({...styles.button, ...style});
      setFullTextStyle({...styles.text, ...styles.textButton, ...styleText});
    }
  }, [disabled, style]);

  return (
    <Pressable style={fullStyle} onPress={onPress} disabled={disabled} >
      <Text style={fullTextStyle}>{children}</Text>
    </Pressable>
  );
}