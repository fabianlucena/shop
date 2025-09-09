import styles from '../libs/styles';
import { View, Text } from 'react-native';

export default function LabelData({
  label,
  data,
  description,
  children,
  style,
}) {
  return <View
      style={{
        ...styles.labelData,
      }}
    >
      {label && <Text style={{...style, flex: 1, ...styles.text, ...styles.label}}>{label}</Text> || null}
      <Text style={{...style, flex: 1, ...styles.text, ...styles.data, ...styles.labelDataData}}>{data}{children}</Text>
      {description && <Text style={{...style, flex: 1, ...styles.text, ...styles.description}}>{description}</Text> || null}
    </View>;
}