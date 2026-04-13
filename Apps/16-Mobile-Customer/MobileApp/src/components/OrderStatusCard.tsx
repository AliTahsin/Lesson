import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { RoomServiceOrder } from '../types/customer';
import { useLanguageContext } from '../context/LanguageContext';

interface Props {
  order: RoomServiceOrder;
}

export const OrderStatusCard: React.FC<Props> = ({ order }) => {
  const { t } = useLanguageContext();

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Pending': return 'time-outline';
      case 'Preparing': return 'restaurant-outline';
      case 'OnTheWay': return 'bicycle-outline';
      case 'Delivered': return 'checkmark-circle-outline';
      case 'Cancelled': return 'close-circle-outline';
      default: return 'ellipse-outline';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Pending': return '#f59e0b';
      case 'Preparing': return '#3b82f6';
      case 'OnTheWay': return '#8b5cf6';
      case 'Delivered': return '#10b981';
      case 'Cancelled': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'Pending': return 'Sipariş Alındı';
      case 'Preparing': return 'Hazırlanıyor';
      case 'OnTheWay': return 'Yolda';
      case 'Delivered': return 'Teslim Edildi';
      case 'Cancelled': return 'İptal Edildi';
      default: return status;
    }
  };

  const formatTime = (dateString?: string) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return `${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <Text style={styles.orderNumber}>{order.orderNumber}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(order.status) }]}>
          <Icon name={getStatusIcon(order.status)} size={12} color="white" />
          <Text style={styles.statusText}>{getStatusText(order.status)}</Text>
        </View>
      </View>

      <View style={styles.itemsContainer}>
        {order.items.map((item, index) => (
          <Text key={index} style={styles.itemText}>
            {item.quantity}x {item.itemName}
          </Text>
        ))}
      </View>

      <View style={styles.footer}>
        <View style={styles.infoRow}>
          <Icon name="time-outline" size={14} color="#666" />
          <Text style={styles.infoText}>
            Sipariş: {formatTime(order.orderTime)}
          </Text>
        </View>
        {order.estimatedDeliveryTime && (
          <View style={styles.infoRow}>
            <Icon name="hourglass-outline" size={14} color="#666" />
            <Text style={styles.infoText}>
              Tahmini: {formatTime(order.estimatedDeliveryTime)}
            </Text>
          </View>
        )}
        <View style={styles.priceRow}>
          <Text style={styles.priceLabel}>Toplam:</Text>
          <Text style={styles.priceValue}>€{order.totalAmount.toFixed(2)}</Text>
        </View>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    padding: 16,
    elevation: 2,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  orderNumber: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
  },
  statusBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
    gap: 4,
  },
  statusText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  itemsContainer: {
    marginBottom: 12,
  },
  itemText: {
    fontSize: 13,
    color: '#555',
    marginBottom: 2,
  },
  footer: {
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
    paddingTop: 12,
  },
  infoRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
    marginBottom: 6,
  },
  infoText: {
    fontSize: 12,
    color: '#666',
  },
  priceRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 8,
  },
  priceLabel: {
    fontSize: 13,
    color: '#666',
  },
  priceValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#007AFF',
  },
});