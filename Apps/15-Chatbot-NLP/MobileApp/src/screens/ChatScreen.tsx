import React, { useState, useEffect, useRef } from 'react';
import {
  View,
  Text,
  FlatList,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  KeyboardAvoidingView,
  Platform,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { chatApi } from '../services/chatApi';
import { signalRService } from '../services/signalRService';
import { ChatBubble } from '../components/ChatBubble';
import { TypingIndicator } from '../components/TypingIndicator';
import { QuickReply } from '../components/QuickReply';
import { ChatMessage, Conversation } from '../types/chat';

export const ChatScreen = ({ route, navigation }: any) => {
  const { hotelId } = route.params || {};
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [inputText, setInputText] = useState('');
  const [loading, setLoading] = useState(true);
  const [sending, setSending] = useState(false);
  const [conversation, setConversation] = useState<Conversation | null>(null);
  const [isTyping, setIsTyping] = useState(false);
  const flatListRef = useRef<FlatList>(null);
  let typingTimeout: NodeJS.Timeout;

  useEffect(() => {
    initChat();
    return () => {
      signalRService.disconnect();
    };
  }, []);

  const initChat = async () => {
    setLoading(true);
    try {
      // Check for existing active conversation
      let activeConv = await chatApi.getActiveConversation();
      
      if (!activeConv) {
        activeConv = await chatApi.startConversation(hotelId);
      }
      
      setConversation(activeConv);
      
      // Load messages
      const msgs = await chatApi.getConversationMessages(activeConv.id);
      setMessages(msgs);
      
      // Connect to SignalR
      await signalRService.connect(activeConv.id);
      
      signalRService.on('NewMessage', (message: ChatMessage) => {
        setMessages(prev => [...prev, message]);
        scrollToBottom();
      });
      
      signalRService.on('UserTyping', (typing: boolean) => {
        setIsTyping(typing);
      });
      
    } catch (error) {
      console.error('Error initializing chat:', error);
      Alert.alert('Hata', 'Sohbet başlatılamadı');
    } finally {
      setLoading(false);
    }
  };

  const scrollToBottom = () => {
    setTimeout(() => {
      flatListRef.current?.scrollToEnd({ animated: true });
    }, 100);
  };

  const handleSend = async () => {
    if (!inputText.trim() || !conversation || sending) return;
    
    setSending(true);
    const messageText = inputText.trim();
    setInputText('');
    
    try {
      await chatApi.sendMessage(conversation.id, messageText);
    } catch (error) {
      console.error('Error sending message:', error);
      Alert.alert('Hata', 'Mesaj gönderilemedi');
    } finally {
      setSending(false);
    }
  };

  const handleInputChange = (text: string) => {
    setInputText(text);
    
    // Send typing indicator
    if (conversation) {
      signalRService.sendTyping(conversation.id, true);
      clearTimeout(typingTimeout);
      typingTimeout = setTimeout(() => {
        signalRService.sendTyping(conversation.id, false);
      }, 1000);
    }
  };

  const handleQuickReply = (text: string) => {
    setInputText(text);
    handleSend();
  };

  const handleEndChat = () => {
    Alert.alert(
      'Sohbeti Sonlandır',
      'Sohbeti sonlandırmak istediğinize emin misiniz?',
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Sonlandır',
          onPress: async () => {
            if (conversation) {
              await chatApi.endConversation(conversation.id);
              navigation.goBack();
            }
          }
        }
      ]
    );
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
      keyboardVerticalOffset={Platform.OS === 'ios' ? 90 : 0}
    >
      <View style={styles.header}>
        <View style={styles.headerLeft}>
          <Icon name="chatbubble-ellipses" size={24} color="#007AFF" />
          <Text style={styles.headerTitle}>Hotel Assistant</Text>
        </View>
        <TouchableOpacity onPress={handleEndChat}>
          <Icon name="close" size={24} color="#666" />
        </TouchableOpacity>
      </View>

      <FlatList
        ref={flatListRef}
        data={messages}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => <ChatBubble message={item} />}
        onContentSizeChange={scrollToBottom}
        onLayout={scrollToBottom}
        contentContainerStyle={styles.messageList}
        ListFooterComponent={isTyping ? <TypingIndicator isTyping={true} /> : null}
      />

      <QuickReply onSelect={handleQuickReply} />

      <View style={styles.inputContainer}>
        <TextInput
          style={styles.input}
          placeholder="Mesajınızı yazın..."
          value={inputText}
          onChangeText={handleInputChange}
          multiline
        />
        <TouchableOpacity
          style={[styles.sendButton, (!inputText.trim() || sending) && styles.sendButtonDisabled]}
          onPress={handleSend}
          disabled={!inputText.trim() || sending}
        >
          {sending ? (
            <ActivityIndicator size="small" color="white" />
          ) : (
            <Icon name="send" size={20} color="white" />
          )}
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  center: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: 'white',
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  headerLeft: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  headerTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  messageList: {
    paddingVertical: 16,
  },
  inputContainer: {
    flexDirection: 'row',
    alignItems: 'flex-end',
    backgroundColor: 'white',
    paddingHorizontal: 12,
    paddingVertical: 8,
    borderTopWidth: 1,
    borderTopColor: '#e0e0e0',
  },
  input: {
    flex: 1,
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 20,
    paddingHorizontal: 16,
    paddingVertical: 8,
    maxHeight: 100,
    fontSize: 16,
  },
  sendButton: {
    backgroundColor: '#007AFF',
    borderRadius: 20,
    width: 40,
    height: 40,
    justifyContent: 'center',
    alignItems: 'center',
    marginLeft: 8,
  },
  sendButtonDisabled: {
    backgroundColor: '#ccc',
  },
});