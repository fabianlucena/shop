import ButtonIcon from './ButtonIcon';

import source from '../images/trash.png';

export default function ButtonIconDelete({
  ...props
}) {
  return <ButtonIcon
      source={source}
      {...props}
    />
}