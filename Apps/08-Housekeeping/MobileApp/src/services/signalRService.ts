import * as signalR from '@microsoft/signalr';

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private listeners: Map<string, Function[]> = new Map();

  async connect(hotelId: number, staffId?: number) {
    const hubUrl = 'http://localhost:5007/hubs/tasks';
    
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log('SignalR connected');

      // Join hotel group
      await this.connection.invoke('JoinHotelGroup', hotelId);
      
      if (staffId) {
        await this.connection.invoke('JoinStaffGroup', staffId);
        await this.connection.invoke('JoinMaintenanceGroup', hotelId);
      }

      // Setup listeners
      this.connection.on('NewTask', (task) => {
        this.emit('NewTask', task);
      });

      this.connection.on('TaskAssigned', (task) => {
        this.emit('TaskAssigned', task);
      });

      this.connection.on('TaskUpdated', (taskId, status) => {
        this.emit('TaskUpdated', { taskId, status });
      });

      this.connection.on('CriticalIssue', (issue) => {
        this.emit('CriticalIssue', issue);
      });

      this.connection.on('IssueUpdated', (issueId, status) => {
        this.emit('IssueUpdated', { issueId, status });
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