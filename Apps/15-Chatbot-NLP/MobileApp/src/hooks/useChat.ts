import { useState, useEffect, useCallback, useRef } from 'react';
import { chatApi } from '../services/chatApi';
import { signalRService } from '../services/signalRService';
import { ChatMessage, Conversation } from '../types/chat';

interface UseChatOptions {
  hotelId?: number;
  autoConnect?: boolean;
}

interface UseChatReturn {
  messages: ChatMessage[];
  conversation: Conversation | null;
  loading: boolean;
  sending: boolean;
  isTyping: boolean;
  sendMessage: (text: string) => Promise<void>;
  sendQuickReply: (text: string) => Promise<void>;
  startNewConversation: () => Promise<void>;
  endConversation: () => Promise<void>;
  loadMessages: (conversationId: number) => Promise<void>;
  clearMessages: () => void;
}

export const useChat = (options: UseChatOptions = {}): UseChatReturn => {
  const { hotelId, autoConnect = true } = options;
  
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [conversation, setConversation] = useState<Conversation | null>(null);
  const [loading, setLoading] = useState(true);
  const [sending, setSending] = useState(false);
  const [isTyping, setIsTyping] = useState(false);
  const typingTimeoutRef = useRef<NodeJS.Timeout>();

  const scrollToBottom = useCallback(() => {
    // Scroll logic would be handled by the component
  }, []);

  const loadMessages = useCallback(async (conversationId: number) => {
    try {
      const msgs = await chatApi.getConversationMessages(conversationId);
      setMessages(msgs);
      scrollToBottom();
    } catch (error) {
      console.error('Error loading messages:', error);
    }
  }, [scrollToBottom]);

  const startNewConversation = useCallback(async () => {
    setLoading(true);
    try {
      const conv = await chatApi.startConversation(hotelId);
      setConversation(conv);
      await loadMessages(conv.id);
      
      // Connect to SignalR
      await signalRService.connect(conv.id);
      
      signalRService.on('NewMessage', (message: ChatMessage) => {
        setMessages(prev => [...prev, message]);
        scrollToBottom();
      });
      
      signalRService.on('UserTyping', (typing: boolean) => {
        setIsTyping(typing);
      });
      
    } catch (error) {
      console.error('Error starting conversation:', error);
    } finally {
      setLoading(false);
    }
  }, [hotelId, loadMessages, scrollToBottom]);

  const sendMessage = useCallback(async (text: string) => {
    if (!text.trim() || !conversation || sending) return;
    
    setSending(true);
    try {
      await chatApi.sendMessage(conversation.id, text.trim());
    } catch (error) {
      console.error('Error sending message:', error);
    } finally {
      setSending(false);
    }
  }, [conversation, sending]);

  const sendQuickReply = useCallback(async (text: string) => {
    await sendMessage(text);
  }, [sendMessage]);

  const endConversation = useCallback(async () => {
    if (!conversation) return;
    
    try {
      await chatApi.endConversation(conversation.id);
      signalRService.disconnect();
      setConversation(null);
      setMessages([]);
    } catch (error) {
      console.error('Error ending conversation:', error);
    }
  }, [conversation]);

  const clearMessages = useCallback(() => {
    setMessages([]);
  }, []);

  // Auto-initialize chat
  useEffect(() => {
    const initChat = async () => {
      setLoading(true);
      try {
        // Check for existing active conversation
        let activeConv = await chatApi.getActiveConversation();
        
        if (!activeConv && autoConnect) {
          activeConv = await chatApi.startConversation(hotelId);
        }
        
        if (activeConv) {
          setConversation(activeConv);
          await loadMessages(activeConv.id);
          
          // Connect to SignalR
          await signalRService.connect(activeConv.id);
          
          signalRService.on('NewMessage', (message: ChatMessage) => {
            setMessages(prev => [...prev, message]);
            scrollToBottom();
          });
          
          signalRService.on('UserTyping', (typing: boolean) => {
            setIsTyping(typing);
          });
        }
      } catch (error) {
        console.error('Error initializing chat:', error);
      } finally {
        setLoading(false);
      }
    };

    if (autoConnect) {
      initChat();
    }

    return () => {
      signalRService.disconnect();
      if (typingTimeoutRef.current) {
        clearTimeout(typingTimeoutRef.current);
      }
    };
  }, [autoConnect, hotelId, loadMessages, scrollToBottom]);

  return {
    messages,
    conversation,
    loading,
    sending,
    isTyping,
    sendMessage,
    sendQuickReply,
    startNewConversation,
    endConversation,
    loadMessages,
    clearMessages,
  };
};