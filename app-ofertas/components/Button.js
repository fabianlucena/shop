import { Pressable } from 'react-native';
import styles from '../libs/styles';
import { Text } from 'react-native';

export default function Button({children, onPress, style}) {
  return (
    <Pressable style={{...styles.button, ...style}} onPress={onPress}  >
      <Text style={{...styles.text, ...styles.textButton}}>{children}</Text>
    </Pressable>
  );
}