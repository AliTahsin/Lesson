import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { Event } from '../types/mice';

interface Props {
  event: Event;
  onPress: (id: number) => void;
}

export const EventCard: React.FC<Props> = ({ event, onPress }) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Planned': return '#f59e0b';
      case 'Confirmed': return '#3b82f6';
      case 'InProgress': return '#8b5cf6';
      case 'Completed': return '#10b981';
      case 'Cancelled': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const getStatusText = (status: string) => {
    switch (status) {
      case 'Planned': return 'Planlandı';
      case 'Confirmed': return 'Onaylandı';
      case 'InProgress': return 'Devam Ediyor';
      case 'Completed': return 'Tamamlandı';
      case 'Cancelled': return 'İptal Edildi';
      default: return status;
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()} ${getMonthName(date.getMonth())} ${date.getFullYear()}`;
  };

  const getMonthName = (month: number) => {
    const months = ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 
                    'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'];
    return months[month];
  };

  const getDaysLeft = () => {
    const start = new Date(event.startDate);
    const today = new Date();
    const diffTime = start.getTime() - today.getTime();
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    return diffDays;
  };

  const daysLeft = getDaysLeft();

  return (
    <TouchableOpacity onPress={() => onPress(event.id)} style={styles.card}>
      <View style={styles.header}>
        <View>
          <Text style={styles.name}>{event.name}</Text>
          <Text style={styles.eventNumber}>{event.eventNumber}</Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(event.status) }]}>
          <Text style={styles.statusText}>{getStatusText(event.status)}</Text>
        </View>
      </View>

      <View style={styles.details}>
        <View style={styles.detailRow}>
          <Icon name="calendar-outline" size={14} color="#666" />
          <Text style={styles.detailText}>
            {formatDate(event.startDate)} - {formatDate(event.endDate)}
          </Text>
        </View>
        <View style={styles.detailRow}>
          <Icon name="people-outline" size={14} color="#666" />
          <Text style={styles.detailText}>{event.expectedAttendees} katılımcı</Text>
        </View>
        <View style={styles.detailRow}>
          <Icon name="business-outline" size={14} color="#666" />
          <Text style={styles.detailText}>{event.customerCompany || event.customerName}</Text>
        </View>
        <View style={styles.detailRow}>
          <Icon name="location-outline" size={14} color="#666" />
          <Text style={styles.detailText}>{event.meetingRoomName}</Text>
        </View>
      </View>

      {daysLeft > 0 && event.status !== 'Completed' && event.status !== 'Cancelled' && (
        <View style={styles.daysLeft}>
          <Text style={styles.daysLeftText}>{daysLeft} gün kaldı</Text>
        </View>
      )}
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
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  name: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  eventNumber: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  details: {
    gap: 6,
  },
  detailRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  detailText: {
    fontSize: 13,
    color: '#666',
  },
  daysLeft: {
    marginTop: 12,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
    alignItems: 'flex-end',
  },
  daysLeftText: {
    fontSize: 12,
    color: '#f59e0b',
    fontWeight: 'bold',
  },
});