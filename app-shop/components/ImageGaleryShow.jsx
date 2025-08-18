import { View } from 'react-native';
import ImageShow from './ImageShow.jsx';

export default function ImageGaleryShow({
  style,
  service,
  images,
}) {
  return <View>
      {images?.map((image, index) => (
        <ImageShow
          key={index}
          style={style}
          service={service}
          image={image}
        />
      )) ?? null}
    </View>;
}