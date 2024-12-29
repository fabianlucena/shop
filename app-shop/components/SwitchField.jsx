import { useEffect, useState } from 'react';
import { View, Switch } from 'react-native';
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
        <Label tyle={labelStyle}>{children}</Label>
        </View>
    </Field>
  );
}