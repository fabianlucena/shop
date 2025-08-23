import { View, ScrollView } from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import Button from './Button';
import ImageShow from './ImageShow';
import ButtonIconDelete from './ButtonIconDelete';
import ButtonIconAdd from './ButtonIconAdd';

export default function ImageGaleryField({
  name,
  value,
  setValue,
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
        {value?.map((image, index) => <View
            key={`${name}-${index}`}
          >
            <ImageShow
              {...image}
              style={{
                width: 200,
                height: 200,
                opacity: image.deleted ? 0.33 : 1,
                backgroundColor: image.deleted ? '#9c0000ff' : 'transparent',
              }}
            />
            {!image.deleted && <ButtonIconDelete
              onPress={() => setValue(value
                .filter(img => img.uri !== image.uri || !img.added)
                ?.map(img => image.uri === img.uri ? { ...img, deleted: true } : img )
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
            {image.deleted && <ButtonIconAdd
              onPress={() => setValue(value
                .map(img => image.uri === img.uri && img.deleted ?
                  { ...img, deleted: undefined }
                  : img
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
              setValue([...value, { uri: result.assets[0].uri, urlBase: '', added: true }]);
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