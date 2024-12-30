import ButtonIcon from './ButtonIcon';

import source from '../images/trash.png';

export default function ButtonIconDelete({
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