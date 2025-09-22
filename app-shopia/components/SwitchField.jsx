import { View, Switch, Pressable } from 'react-native';
import Field from './Field';
import Label from './Label';
import styles from '../libs/styles';

export default function TextField({
  children,
  value,
  onChangeValue,
  disabled = false,
  style,
  inputStyle,
  labelStyle,
}) {
  return (
    <Field style={style}>
      <View style={styles.sameLine}>
        <Switch
          value={value}
          disabled={disabled}
          onValueChange={onChangeValue}
          style={{...inputStyle}}
        />
        <Pressable
          onPress={() => onChangeValue(!value)}
        >
          <Label tyle={labelStyle}>{children}</Label>
        </Pressable>
        </View>
    </Field>
  );
}