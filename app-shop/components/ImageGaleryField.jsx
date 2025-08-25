import { useState } from 'react';
import { View } from 'react-native';
import Button from './Button';
import ButtonIconDelete from './ButtonIconDelete';
import ButtonIconAdd from './ButtonIconAdd';
import { getImageFrom } from '../libs/images';
import ImageGaleryShow from './ImageGaleryShow';

export default function ImageGaleryField({
  name,
  value,
  setValue,
  aspect = [9, 16],
  maxWidth = 1080,
  maxHeight = 1920,
}) {
  const [width, setWidth] = useState(0);

  async function addImageFrom(source) {
    const image = await getImageFrom({ source, aspect, maxWidth, maxHeight });
    if (image)
      setValue([...value, {...image, added: true, urlBase: ''}]);  
  }

  return <View
      onLayout={event => {
        setWidth(Math.floor(event.nativeEvent.layout.width));
      }}
    >
      <ImageGaleryShow
        images={value.map(image => ({
          ...image,
          ...(image.deleted && {
            style: {
              ...image.style,
              opacity: 0.75,
              backgroundColor: '#FF000080',
            },
            overlay: <ButtonIconAdd
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
            />,
          }),
          ...(!image.deleted && {
            overlay: <ButtonIconDelete
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
            />
          })
        }))}
        containerStyle={{
          width: width - 24,
        }}
        style={{
          aspectRatio: 9 / 16,
          borderRadius: 8,
          width: width * 2 / 3 - 24,
        }}
        canFullScreen={false}
        autoSlide={false}
      />
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