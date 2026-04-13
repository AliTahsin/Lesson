import * as signalR from '@microsoft/signalr';
import { storage } from '../utils/storage';

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private listeners: Map<string, Function[]> = new Map();

  async connect(hotelId: number, staffId: number) {
    const token = await storage.getAccessToken();
    const hubUrl = 'http://localhost:5016/hubs/staff';
    
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => token || ''
      })
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log('SignalR connected');

      await this.connection.invoke('JoinHotelGroup', hotelId);
      await this.connection.invoke('JoinStaffGroup', staffId);

      // Setup listeners
      this.connection.on('NewTask', (task) => {
        this.emit('NewTask', task);
      });

      this.connection.on('TaskAssigned', (task) => {
        this.emit('TaskAssigned', task);
      });

      this.connection.on('CriticalIssue', (issue) => {
        this.emit('CriticalIssue', issue);
      });

      this.connection.on('IssueAssigned', (issue) => {
        this.emit('IssueAssigned', issue);
      });

      this.connection.on('NewNotification', (notification) => {
        this.emit('NewNotification', notification);
      });

      this.connection.on('GuestCheckedIn', (checkIn) => {
        this.emit('GuestCheckedIn', checkIn);
      });

      this.connection.on('GuestCheckedOut', (checkOut) => {
        this.emit('GuestCheckedOut', checkOut);
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