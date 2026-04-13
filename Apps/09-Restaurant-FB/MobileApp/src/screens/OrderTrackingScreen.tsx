import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { orderApi } from '../services/orderApi';
import { signalRService } from '../services/signalRService';
import { Order } from '../types/restaurant';

export const OrderTrackingScreen = ({ route, navigation }: any) => {
  const { orderId, orderNumber } = route.params;
  const [order, setOrder] = useState<Order | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadOrder();
    setupSignalR();
    return () => {
      signalRService.disconnect();
    };
  }, []);

  const loadOrder = async () => {
    setLoading(true);
    try {
      const data = await orderApi.getOrderById(orderId);
      setOrder(data);
    } catch (error) {
      console.error('Error loading order:', error);
    } finally {
      setLoading(false);
    }
  };

  const setupSignalR = async () => {
    if (!order) return;
    await signalRService.connect(order.restaurantId);
    
    signalRService.on('OrderStatusUpdated', ({ orderId: updatedId, status }) => {
      if (updatedId === orderId) {
        loadOrder();
      }
    });
  };

  const getStatusStep = (status: string) => {
    const steps = ['Pending', 'Preparing', 'Ready', 'Served', 'Completed'];
    return steps.indexOf(status);
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Pending': return 'time-outline';
      case 'Preparing': return 'restaurant-outline';
      case 'Ready': return 'checkmark-circle-outline';
      case 'Served': return 'happy-outline';
      case 'Completed': return 'checkmark-done-circle-outline';
      default: return 'ellipse-outline';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Pending': return '#f59e0b';
      case 'Preparing': return '#3b82f6';
      case 'Ready': return '#10b981';
      case 'Served': return '#8b5cf6';
      case 'Completed': return '#16a34a';
      default: return '#9ca3af';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'Pending': return 'Sipariş Alındı';
      case 'Preparing': return 'Hazırlanıyor';
      case 'Ready': return 'Hazır';
      case 'Served': return 'Servis Edildi';
      case 'Completed': return 'Tamamlandı';
      default: return status;
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!order) {
    return (
      <View style={styles.center}>
        <Text>Sipariş bulunamadı</Text>
      </View>
    );
  }

  const currentStep = getStatusStep(order.status);

  return (
    <View style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.orderNumber}>Sipariş No: {order.orderNumber}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(order.status) }]}>
          <Text style={styles.statusText}>{getStatusText(order.status)}</Text>
        </View>
      </View>

      <View style={styles.timeline}>
        {['Pending', 'Preparing', 'Ready', 'Served', 'Completed'].map((status, index) => (
          <View key={status} style={styles.timelineStep}>
            <View style={styles.timelineLeft}>
              <View style={[
                styles.timelineDot,
                currentStep >= index && styles.timelineDotActive,
                { backgroundColor: currentStep >= index ? getStatusColor(status) : '#e0e0e0' }
              ]}>
                <Icon
                  name={getStatusIcon(status)}
                  size={16}
                  color={currentStep >= index ? 'white' : '#999'}
                />
              </View>
              {index < 4 && (
                <View style={[
                  styles.timelineLine,
                  currentStep > index && styles.timelineLineActive
                ]} />
              )}
            </View>
            <View style={styles.timelineRight}>
              <Text style={[
                styles.timelineTitle,
                currentStep >= index && styles.timelineTitleActive
              ]}>
                {getStatusText(status)}
              </Text>
              {status === 'Preparing' && order.preparationStartTime && (
                <Text style={styles.timelineTime}>
                  {new Date(order.preparationStartTime).toLocaleTimeString()}
                </Text>
              )}
              {status === 'Ready' && order.readyTime && (
                <Text style={styles.timelineTime}>
                  {new Date(order.readyTime).toLocaleTimeString()}
                </Text>
              )}
              {status === 'Served' && order.servedTime && (
                <Text style={styles.timelineTime}>
                  {new Date(order.servedTime).toLocaleTimeString()}
                </Text>
              )}
            </View>
          </View>
        ))}
      </View>

      <View style={styles.orderDetails}>
        <Text style={styles.detailsTitle}>Sipariş Detayı</Text>
        {order.items.map((item, index) => (
          <View key={index} style={styles.orderItem}>
            <Text style={styles.itemName}>{item.quantity}x {item.itemName}</Text>
            <Text style={styles.itemPrice}>€{item.totalPrice.toFixed(2)}</Text>
          </View>
        ))}
        <View style={styles.divider} />
        <View style={styles.totalRow}>
          <Text style={styles.totalLabel}>Toplam</Text>
          <Text style={styles.totalValue}>€{order.totalAmount.toFixed(2)}</Text>
        </View>
      </View>

      {order.status !== 'Completed' && order.status !== 'Cancelled' && (
        <TouchableOpacity
          style={styles.cancelButton}
          onPress={() => {
            Alert.alert(
              'Sipariş İptali',
              'Siparişinizi iptal etmek istediğinize emin misiniz?',
              [
                { text: 'Hayır', style: 'cancel' },
                {
                  text: 'Evet',
                  onPress: async () => {
                    await orderApi.cancelOrder(order.id, 'Kullanıcı iptali');
                    loadOrder();
                  }
                }
              ]
            );
          }}
        >
          <Text style={styles.cancelButtonText}>Siparişi İptal Et</Text>
        </TouchableOpacity>
      )}

      <TouchableOpacity
        style={styles.backButton}
        onPress={() => navigation.navigate('RestaurantList')}
      >
        <Text style={styles.backButtonText}>Ana Menüye Dön</Text>
      </TouchableOpacity>
    </View>
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
  headerCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  orderNumber: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
  timeline: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    padding: 20,
  },
  timelineStep: {
    flexDirection: 'row',
    marginBottom: 20,
  },
  timelineLeft: {
    width: 40,
    alignItems: 'center',
  },
  timelineDot: {
    width: 32,
    height: 32,
    borderRadius: 16,
    alignItems: 'center',
    justifyContent: 'center',
    zIndex: 1,
  },
  timelineDotActive: {
    elevation: 3,
  },
  timelineLine: {
    width: 2,
    height: 40,
    backgroundColor: '#e0e0e0',
    marginTop: 4,
  },
  timelineLineActive: {
    backgroundColor: '#10b981',
  },
  timelineRight: {
    flex: 1,
    marginLeft: 12,
  },
  timelineTitle: {
    fontSize: 14,
    fontWeight: '500',
    color: '#999',
  },
  timelineTitleActive: {
    color: '#333',
    fontWeight: 'bold',
  },
  timelineTime: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  orderDetails: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  detailsTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  orderItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  itemName: {
    fontSize: 14,
    color: '#555',
  },
  itemPrice: {
    fontSize: 14,
    color: '#555',
  },
  divider: {
    height: 1,
    backgroundColor: '#f0f0f0',
    marginVertical: 12,
  },
  totalRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  totalLabel: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  totalValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  cancelButton: {
    backgroundColor: '#ef4444',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    paddingVertical: 14,
    alignItems: 'center',
  },
  cancelButtonText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold',
  },
  backButton: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 14,
    alignItems: 'center',
  },
  backButtonText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold',
  },
});