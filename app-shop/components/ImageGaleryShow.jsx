import { View } from 'react-native';
import ImageShow from './ImageShow.jsx';

export default function ImageGaleryShow({
  images,
  ...props
}) {
  return <View
      style={{
        flexDirection: 'row',
        flexWrap: 'wrap',
        justifyContent: 'space-between',
      }}
    >
      {images?.map((image, index) => (
        <ImageShow
          key={index}
          image={image}
          {...props}
        />
      )) ?? null}
    </View>;
}