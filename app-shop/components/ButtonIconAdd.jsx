import ButtonIcon from './ButtonIcon';

import source from '../images/add.png';

export default function ButtonIconAdd({
  onPress,
  navigate,
  disabled,
  alt,
  size,
  style,
  styleIcon,
}) {
  return <ButtonIcon
      onPress={onPress}
      disabled={disabled}
      navigate={navigate}
      alt={alt}
      source={source}
      size={size}
      style={style}
      styleIcon={styleIcon}
    />
}