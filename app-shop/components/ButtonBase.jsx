import { Pressable } from 'react-native';
import { useNavigation } from '@react-navigation/native'

import styles from '../libs/styles';

export default function Button({
  children,
  onPress,
  navigate,
  disabled,
  style,
}) {
  const navigation = useNavigation();

  function handleOnPress(evt) {
    if (onPress) {
      onPress(evt);
      return;
    }

    if (navigate && navigation) {
      if (Array.isArray(navigate))
        navigation.navigate(...navigate);
      else
        navigation.navigate(navigate);

      return;
    }
  }

  return (
    <Pressable
      onPress={handleOnPress}
      disabled={disabled}
      style={{
        ...styles.button,
        ...style,
        ...(disabled && styles.disabledButton || null),
      }}
    >
      {children}
    </Pressable>
  );
}