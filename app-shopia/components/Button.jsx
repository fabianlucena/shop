import { Text } from 'react-native';

import styles from '../libs/styles';

import ButtonBase from './ButtonBase';

export default function Button({
  children,
  onPress,
  navigate,
  disabled,
  style,
  styleText,
}) {
  return <ButtonBase
      onPress={onPress}
      navigate={navigate}
      disabled={disabled}
      style={style}
    >
      <Text
        style={{
          ...styles.text,
          ...styles.textButton,
          ...styleText,
          ...(disabled && styles.disabledTextButton || null),
        }}
      >
        {children}
      </Text>
    </ButtonBase>
}