import * as ImagePicker from 'expo-image-picker';
import * as ImageManipulator from 'expo-image-manipulator';

export async function getImageFrom({
  source,
  aspect,
  quality,
  allowsEditing,
  maxWidth,
  maxHeight,
}) {
  let method;
  if (source === 'camera')
    method = 'launchCameraAsync';
  else if (source === 'library')
    method = 'launchImageLibraryAsync';
  else
    throw new Error('Invalid image source');

  let result = await ImagePicker[method]({
    mediaTypes: ['images'],
    allowsEditing: allowsEditing ?? true,
    aspect,
    quality: quality ?? 1,
  });

  if (result.canceled)
    return;

  let image = result.assets[0];
  image = await resizeIfNeeded(image, maxWidth, maxHeight);

  return image;
}

export async function resizeIfNeeded(image, maxWidth, maxHeight) {
    if (!maxWidth) {
      if (!maxHeight || !aspect || aspect.length !== 2)
        return image;

      maxWidth = Math.round((maxHeight * aspect[0]) / aspect[1]);
    } else if (!maxHeight) {
      if (!aspect || aspect.length !== 2)
        return image;

      maxHeight = Math.round((maxWidth * aspect[1]) / aspect[0]);
    }

    if (image.width <= maxWidth && image.height <= maxHeight)
      return image;

    let newWidth,
      newHeight;
    if (image.width > maxWidth) {
      newWidth = maxWidth;
      newHeight = Math.round((image.height * maxWidth) / image.width);
    }

    if (image.height > maxHeight) {
      newWidth = Math.round((image.width * maxHeight) / image.height);
      newHeight = maxHeight;
 
      if (image.width > maxWidth) {
        newWidth = maxWidth;
        newHeight = Math.round((image.height * maxWidth) / image.width);
      } 
    }

    if (image.width <= maxWidth && image.height <= maxHeight)
      return image;

    try {
      image = await ImageManipulator.manipulateAsync(
        image.uri,
        [{ resize: { width: newWidth, height: newHeight } }],
        { compress: 0.9, format: ImageManipulator.SaveFormat.JPEG }
      );
    } catch(e) {
      console.error(e);
      return;
    }

    return image;
  }