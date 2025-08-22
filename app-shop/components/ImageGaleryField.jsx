import { View, ScrollView, Text } from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import Button from './Button';
import ImageShow from './ImageShow';
import ButtonIconDelete from './ButtonIconDelete';
import ButtonIconAdd from './ButtonIconAdd';

export default function ImageGaleryField({
  name,
  value,
  setValue,
  service,
}) {
  return <View>
      <ScrollView
        horizontal
        contentContainerStyle={{
          gap: 12,
          margin: 12,
          paddingRight: 24,
        }}
      >
        {value?.map((data, index) => <View
            key={`${name}-${index}`}
          >
            <ImageShow
              {...data}
              service={service}
              style={{
                width: 200,
                height: 200,
                opacity: data.deleted ? 0.33 : 1,
                backgroundColor: data.deleted ? '#9c0000ff' : 'transparent',
              }}
            />
            {!data.deleted && <ButtonIconDelete
              onPress={() => setValue(value
                .filter(image => !data.uri || image.uri !== data.uri)
                ?.map(image => data.image && data.image === image.image ? { ...image, deleted: true } : image )
              )}
              style={{
                position: 'absolute',
                top: 0,
                right: 0,
                margin: 6,
                backgroundColor: 'rgba(165, 17, 17, 0.51)',
              }}
              styleIcon={{
                marginTop: 3,
                marginLeft: 6,
                marginRight: 6,
                marginBottom: 3,
              }}
            /> || null}
            {data.deleted && <ButtonIconAdd
              onPress={() => setValue(value
                .map(image => data.image && data.image === image.image && image.deleted ?
                  { ...image, deleted: undefined }
                  : image
                )
              )}
              style={{
                position: 'absolute',
                top: 0,
                right: 0,
                margin: 6,
                backgroundColor: 'rgba(19, 17, 165, 0.51)',
              }}
              styleIcon={{
                marginTop: 3,
                marginLeft: 6,
                marginRight: 6,
                marginBottom: 3,
              }}
            /> || null}
          </View>
        ) || null}
      </ScrollView>
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
              setValue([...value, { uri: result.assets[0].uri }]);
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