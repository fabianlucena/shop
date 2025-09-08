import styles from '../libs/styles';
import { View, Text } from 'react-native';

export default function LabelData({
  label,
  data,
  children,
  style,
}) {
  return <View
      style={{
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      {label && <Text style={{flex: 1, ...styles.label, ...style}}>{label}</Text> || null}
      <Text style={{flex: 1, ...styles.text, ...style}}>{data}{children}</Text>
    </View>;
}