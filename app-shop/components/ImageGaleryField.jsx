import { View, ScrollView } from 'react-native';
import Button from './Button';
import ImageShow from './ImageShow';
import ButtonIconDelete from './ButtonIconDelete';
import ButtonIconAdd from './ButtonIconAdd';
import { getImageFrom } from '../libs/images';

export default function ImageGaleryField({
  name,
  value,
  setValue,
  aspect = [9, 16],
  maxWidth = 1080,
  maxHeight = 1920,
}) {
  async function addImageFrom(source) {
    const image = await getImageFrom({ source, aspect, maxWidth, maxHeight });
    if (image)
      setValue([...value, {...image, added: true, urlBase: ''}]);  
  }

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
          onPress={() => addImageFrom('library')}
        >
          Seleccionar imagen
        </Button>
        <Button
          onPress={() => addImageFrom('camera')}
        >
          Tomar foto
        </Button>
      </View>
    </View>;
}