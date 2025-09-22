import { useEffect, useState } from 'react';
import { View } from 'react-native';
import CurrencyInput from 'react-native-currency-input';
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
  labelStyle
}) {
  return (
    <Field style={style}>
      <View style={styles.sameLine}>
        <Label tyle={labelStyle}>{children}</Label>
      </View>
      <CurrencyInput
        style={{...styles.input, ...inputStyle}}
        value={value}
        disabled={disabled}
        onChangeValue={onChangeValue}
        prefix="$ "
        delimiter="."
        separator=","
        precision={2}
      />
    </Field>
  );
}