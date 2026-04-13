import React, { createContext, useContext, ReactNode } from 'react';
import { useChat } from '../hooks/useChat';
import { ChatMessage, Conversation } from '../types/chat';

interface ChatContextType {
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

const ChatContext = createContext<ChatContextType | undefined>(undefined);

interface ChatProviderProps {
  children: ReactNode;
  hotelId?: number;
  autoConnect?: boolean;
}

export const ChatProvider: React.FC<ChatProviderProps> = ({ 
  children, 
  hotelId, 
  autoConnect = true 
}) => {
  const chat = useChat({ hotelId, autoConnect });

  return (
    <ChatContext.Provider value={chat}>
      {children}
    </ChatContext.Provider>
  );
};

export const useChatContext = () => {
  const context = useContext(ChatContext);
  if (context === undefined) {
    throw new Error('useChatContext must be used within a ChatProvider');
  }
  return context;
};