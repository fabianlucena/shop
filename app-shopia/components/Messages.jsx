import styles from '../libs/styles';
import { View, Text, Pressable } from 'react-native';
import useSession from '../contexts/Session';

export default function Messages() {
  const { messages, setMessages } = useSession();

  if (!messages.length)
    return null;

  return <View
      style={styles.messagesContainer}
    >
      {messages.map((message, index) => 
        <Pressable
          key={index}
          onPress={() => setMessages(messages => messages.filter((_, i) => i !== index))}
        >
          <Text
            style={{
              ...styles.text,
              ...styles.message,
              ...(message.variant === 'error' ? styles.error : {}),
              ...message.style
            }}
          >
            {message.message}
          </Text>
        </Pressable>
      )}
    </View>;
}