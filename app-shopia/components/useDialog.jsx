import { Alert } from 'react-native';

const isBrowser = typeof window !== 'undefined';

export default function useDialog() {
  function confirm({
    title = 'Confirmación',
    message = '¿Estás seguro de que deseas continuar?',
    onOk = null,
    onCancel = null,
  }) {
    if (isBrowser) {
      new Promise(() => {
        if (window.confirm(message))
          onOk && onOk()
        else
          onCancel && onCancel()
      });

      return;
    }

    Alert.alert(
      title,
      message,
      [
        {
          text: 'Cancelar',
          onPress: () => { onCancel && onCancel() },
          style: 'cancel'
        },
        {
          text: 'Aceptar',
          onPress: () => { onOk && onOk() },
        }
      ],
      { cancelable: false }
    );
  }

  function message({
    title = 'Mensaje',
    message = '',
  }) {
    if (isBrowser) {
      new Promise(() => window.alert(message));
      return;
    }

    Alert.alert(
      title,
      message,
    );
  }

  return {
    confirm,
    message,
  };
}
