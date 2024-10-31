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
    backgroundColor: '#e0e0f0',
  },

  text: {
    fontSize: 22,
  },

  hint: {
    fontSize: 16,
    color: '#404040',
    borderWidth: 2,
    borderRadius: 10,
    borderColor: '#a0a0a0',
    backgroundColor: '#e0e0e0',
    paddingVertical: 6,
    paddingHorizontal: 10,
    width: '100%',
  },

  button: {
    borderBottomWidth: 2,
    borderBottomColor: '#b0b0b0',
    paddingVertical: 3,
    paddingHorizontal: 12,
    margin: 5,
  },

  disabledButton: {
  },

  textButton: {
    fontSize: 16,
    textAlign: 'center',
  },

  disabledTextButton: {
    color: '#606060',
  },

  field: {
    width: '100%',
    margin: 4,
    padding: 5,
  },

  input: {
    fontSize: 26,
    borderBottomWidth: 2,
    borderBottomColor: '#a0a0a0',
    borderRadius: 2,
  },

  label: {
    color: '#606060',
    fontSize: 20,
  },

  sameLine: {
    display: 'flex',
    flexDirection: 'row',
    alignItems: 'center',
    gap: 10,
  },
});

export default styles;
