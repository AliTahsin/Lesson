import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { Service } from '../types/gateway';
import { HealthBadge } from './HealthBadge';

interface Props {
  service: Service;
  onPress: (service: Service) => void;
}

export const ServiceCard: React.FC<Props> = ({ service, onPress }) => {
  const getServiceIcon = (name: string) => {
    const icons: Record<string, string> = {
      HotelManagement: '🏨',
      RoomManagement: '🛏️',
      ReservationSystem: '📅',
      DynamicPricing: '💰',
      CRMLoyalty: '👥',
      ChannelManagement: '📡',
      PaymentInvoice: '💳',
      Housekeeping: '🧹',
      RestaurantFB: '🍽️',
      MICEMeeting: '🏛️',
      ReportingBI: '📊',
      AuthRBAC: '🔐',
      NotificationPush: '📢',
      AIRecommendation: '🤖',
      ChatbotNLP: '💬',
      MobileCustomer: '📱',
      MobileStaff: '👔'
    };
    return icons[name] || '⚙️';
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
  };

  return (
    <TouchableOpacity style={styles.card} onPress={() => onPress(service)}>
      <View style={styles.header}>
        <View style={styles.iconContainer}>
          <Text style={styles.icon}>{getServiceIcon(service.name)}</Text>
        </View>
        <View style={styles.info}>
          <Text style={styles.name}>{service.name}</Text>
          <Text style={styles.url}>{service.url}</Text>
        </View>
        <HealthBadge isHealthy={service.isHealthy} />
      </View>

      <View style={styles.details}>
        <View style={styles.detailRow}>
          <Icon name="server-outline" size={14} color="#666" />
          <Text style={styles.detailText}>Port: {service.port}</Text>
        </View>
        <View style={styles.detailRow}>
          <Icon name="time-outline" size={14} color="#666" />
          <Text style={styles.detailText}>Son Heartbeat: {formatDate(service.lastHeartbeat)}</Text>
        </View>
        <View style={styles.detailRow}>
          <Icon name="calendar-outline" size={14} color="#666" />
          <Text style={styles.detailText}>Kayıt: {formatDate(service.registeredAt)}</Text>
        </View>
      </View>
    </TouchableOpacity>
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
    alignItems: 'center',
    marginBottom: 12,
  },
  iconContainer: {
    width: 48,
    height: 48,
    borderRadius: 24,
    backgroundColor: '#f0f0f0',
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 12,
  },
  icon: {
    fontSize: 24,
  },
  info: {
    flex: 1,
  },
  name: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  url: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  details: {
    marginTop: 8,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  detailRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 6,
    gap: 6,
  },
  detailText: {
    fontSize: 12,
    color: '#666',
  },
});