import * as signalR from '@microsoft/signalr';

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private listeners: Map<string, Function[]> = new Map();

  async connect(restaurantId: number) {
    const hubUrl = 'http://localhost:5008/hubs/restaurant';
    
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log('SignalR connected for restaurant:', restaurantId);

      // Join restaurant group
      await this.connection.invoke('JoinRestaurantGroup', restaurantId);

      // Setup listeners
      this.connection.on('NewOrder', (order) => {
        this.emit('NewOrder', order);
      });

      this.connection.on('OrderUpdated', (order) => {
        this.emit('OrderUpdated', order);
      });

      this.connection.on('OrderStatusUpdated', (orderId, status) => {
        this.emit('OrderStatusUpdated', { orderId, status });
      });

      this.connection.on('NewReservation', (reservation) => {
        this.emit('NewReservation', reservation);
      });

      this.connection.on('StockAlert', (alerts) => {
        this.emit('StockAlert', alerts);
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