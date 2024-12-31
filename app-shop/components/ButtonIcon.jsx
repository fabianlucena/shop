import { Image } from 'react-native';

import styles from '../libs/styles';

import ButtonBase from './ButtonBase';
import Background from './Background';

export default function ButtonIcon({
  onPress,
  navigate,
  disabled,
  alt,
  source,
  style,
  styleIcon,
  size = 'medium',
}) {
  return <ButtonBase
      onPress={onPress}
      disabled={disabled}
      navigate={navigate}
      style={{
        ...styles.buttonIcon,
        ...style,
      }}
    >
      <Image
        alt={alt}
        style={{
          ...styles.icon,
          ...styles.mediumIcon,
          ...styles[size + 'Icon'],
          ...styleIcon,
        }}
        resizeMode="contain"
        source={source}
      />
    </ButtonBase>
}