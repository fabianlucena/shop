import { View, Switch } from 'react-native';
import RNPickerSelect from 'react-native-picker-select';

import Field from './Field';
import Label from './Label';
import styles from '../libs/styles';

export default function SelectField({
  children,
  options,
  value,
  onChangeValue,
  style,
  labelStyle,
}) {
  return (
    <Field style={style}>
      <View style={styles.sameLine}>
        <Label tyle={labelStyle}>{children}</Label>
      </View>
      <RNPickerSelect
        value={value}
        onValueChange={onChangeValue}
        items={options}
        style={{ inputIOS: styles.input, inputAndroid: styles.input, }}
        placeholder={{ label: 'Selecciona una opción...', value: null, }}
      />
    </Field>
  );
}