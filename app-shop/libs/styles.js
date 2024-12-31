import { StyleSheet } from 'react-native';

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    flexGrow: 1,
    flexDirection: 'column',
    padding: 10,
  },

  background: {
    flexGrow: 1,
    flexShrink: 1,
    backgroundColor: '#b5f6a7',
  },

  loader: {
    color: '#29681b',
  },

  busy: {
    flexGrow: 1,
    flexShrink: 1,
  },

  text: {
    fontSize: 22,
  },

  messageContainer: {
    width: '100%',
  },

  message: {
    fontSize: 16,
    color: '#13310d',
    borderWidth: 2,
    borderRadius: 10,
    borderColor: '#29681b',
    backgroundColor: '#daf9d4',
    paddingVertical: 6,
    paddingHorizontal: 10,
    margin: '.5em',
  },

  error: {
    color: '#421010',
    borderColor: '#601717',
    backgroundColor: '#fac8c8',
  },

  button: {
    backgroundColor: '#29681b',
    borderRadius: '1em',
    boxShadow: '.2em .1em .2em #404040',
    paddingVertical: 5,
    paddingHorizontal: 18,
    margin: 5,
  },

  disabledButton: {
  },

  textButton: {
    fontSize: 18,
    textAlign: 'center',
    color: '#ffffff',
  },

  disabledTextButton: {
    color: '#d0d0d0',
  },

  field: {
    width: '100%',
    margin: 4,
    padding: 5,
  },

  input: {
    fontSize: 26,
    borderBottomWidth: 2,
    borderBottomColor: '#29681b',
    color: '#194210',
    borderRadius: 2,
    backgroundColor: '#daf9d4',
  },

  label: {
    color: '#194210',
    fontSize: 18,
  },

  header: {
    color: '#194210',
    fontSize: 26,
    fontWeight: 'bold',
    margin: 10,
  },

  sameLine: {
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'center',
    gap: 10,
  },

  busyIndicatorContainer: {
    position: 'absolute',
    top: 0,
    left: 0,
    width: '100%',
    height: '100%',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: 'rgba(232, 224, 240, .5)',
  },

  busyIndicator: {
    width: '25%',
    height: '25%',
  },

  item: {
    borderWidth: 2,
    borderColor: '#29681b',
    backgroundColor: '#daf9d4',
    borderRadius: '1em',
    padding: '.5em',
    alignItems: 'center',
  },

  itemHeader: {
    fontSize: 18,
    margin: '.2em',
  },

  buttonIcon: {
    backgroundColor: 'rgba(128, 128, 128, 0)',
    boxShadow: 'none',
    margin: 0,
    padding: 0,
    paddingVertical: 0,
    paddingHorizontal: 0,
  },

  mediumIcon: {
    width: 22,
  },

  smallIcon: {
    width: 12,
  },

  bigIcon: {
    width: 34,
  },
});

export default styles;
