import { View, Image } from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import Button from './Button';

export default function ImageGaleryField({
  name,
  value,
  setValue,
}) {
  return <View>
      {value?.length && 
        value.map((uri, index) => (
          <Image
            key={`${name}-${index}`}
            source={{ uri }}
            style={{ width: 200, height: 200, marginBottom: 10 }}
          />
        ))
      || null}
      <View style={{ flexDirection: 'row', justifyContent: 'space-between', marginTop: 10 }}>
        <Button
          onPress={async () => {
            let result = await ImagePicker.launchImageLibraryAsync({
              mediaTypes: ['images'],
              allowsEditing: true,
              aspect: [4, 3],
              quality: 1,
            });

            if (!result.canceled) {
              setValue([...value, result.assets[0].uri]);
            }
          }}
        >
          Seleccionar imagen
        </Button>
        <Button
          onPress={async () => {
            let result = await ImagePicker.launchCameraAsync({
              mediaTypes: ['images'],
              allowsEditing: true,
              aspect: [4, 3],
              quality: 1,
            });

            if (!result.canceled) {
              setValue([...value, result.assets[0].uri]);
            }
          }}
        >
          Tomar foto
        </Button>
      </View>
    </View>;
}