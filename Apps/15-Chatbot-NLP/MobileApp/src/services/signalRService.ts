import * as signalR from '@microsoft/signalr';
import AsyncStorage from '@react-native-async-storage/async-storage';

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private listeners: Map<string, Function[]> = new Map();

  async connect(conversationId: number) {
    const token = await AsyncStorage.getItem('accessToken');
    const hubUrl = 'http://localhost:5014/hubs/chat';
    
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => token || ''
      })
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log('SignalR connected');

      // Join chat group
      await this.connection.invoke('JoinChat', conversationId);

      // Setup listeners
      this.connection.on('NewMessage', (message) => {
        this.emit('NewMessage', message);
      });

      this.connection.on('UserTyping', (isTyping) => {
        this.emit('UserTyping', isTyping);
      });

    } catch (error) {
      console.error('SignalR connection error:', error);
    }
  }

  disconnect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }

  async sendTyping(conversationId: number, isTyping: boolean) {
    if (this.connection) {
      await this.connection.invoke('Typing', conversationId, isTyping);
    }
  }

  on(event: string, callback: Function) {
    if (!this.listeners.has(event)) {
      this.listeners.set(event, []);
    }
    this.listeners.get(event)!.push(callback);
  }

  off(event: string, callback: Function) {
    const callbacks = this.listeners.get(event);
    if (callbacks) {
      const index = callbacks.indexOf(callback);
      if (index !== -1) callbacks.splice(index, 1);
    }
  }

  private emit(event: string, data: any) {
    const callbacks = this.listeners.get(event);
    if (callbacks) {
      callbacks.forEach(callback => callback(data));
    }
  }
}

export const signalRService = new SignalRService();