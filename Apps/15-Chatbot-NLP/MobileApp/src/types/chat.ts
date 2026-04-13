export interface ChatMessage {
  id: number;
  conversationId: number;
  senderType: string; // User, Bot, Agent
  senderId?: number;
  senderName: string;
  message: string;
  intent?: string;
  status: string;
  sentAt: string;
}

export interface Conversation {
  id: number;
  userId?: number;
  userName: string;
  userEmail: string;
  hotelId?: number;
  status: string;
  assignedAgentId?: number;
  assignedAgentName?: string;
  startedAt: string;
  endedAt?: string;
  lastMessageAt?: string;
  messageCount: number;
  isBotActive: boolean;
}

export interface SendMessageRequest {
  conversationId: number;
  message: string;
}

export interface StartConversationRequest {
  hotelId?: number;
}

export interface ChatStatistics {
  totalConversations: number;
  activeConversations: number;
  resolvedConversations: number;
  averageResponseTime: number;
  satisfactionRate: number;
}