import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { Notification } from '../types/notification';

interface Props {
  notification: Notification;
  onPress: (id: number) => void;
  onDelete?: (id: number) => void;
}

export const NotificationCard: React.FC<Props> = ({ notification, onPress, onDelete }) => {
  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'Info': return 'information-circle';
      case 'Success': return 'checkmark-circle';
      case 'Warning': return 'warning';
      case 'Error': return 'close-circle';
      case 'Promo': return 'gift';
      default: return 'notifications';
    }
  };

  const getTypeColor = (type: string) => {
    switch (type) {
      case 'Info': return '#3b82f6';
      case 'Success': return '#10b981';
      case 'Warning': return '#f59e0b';
      case 'Error': return '#ef4444';
      case 'Promo': return '#ec4899';
      default: return '#6b7280';
    }
  };

  const getCategoryIcon = (category: string) => {
    switch (category) {
      case 'Reservation': return '📅';
      case 'Payment': return '💰';
      case 'Housekeeping': return '🧹';
      case 'Promo': return '🎁';
      default: return '📢';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    const diffHours = Math.floor(diffMs / 3600000);
    const diffDays = Math.floor(diffMs / 86400000);

    if (diffMins < 1) return 'Şimdi';
    if (diffMins < 60) return `${diffMins} dakika önce`;
    if (diffHours < 24) return `${diffHours} saat önce`;
    return `${diffDays} gün önce`;
  };

  return (
    <TouchableOpacity
      style={[styles.card, !notification.isRead && styles.unreadCard]}
      onPress={() => onPress(notification.id)}
    >
      <View style={styles.header}>
        <View style={[styles.typeIcon, { backgroundColor: getTypeColor(notification.type) + '20' }]}>
          <Icon name={getTypeIcon(notification.type)} size={20} color={getTypeColor(notification.type)} />
        </View>
        <View style={styles.headerInfo}>
          <Text style={styles.title}>{notification.title}</Text>
          <Text style={styles.category}>
            {getCategoryIcon(notification.category)} {notification.category}
          </Text>
        </View>
        {!notification.isRead && <View style={styles.unreadDot} />}
      </View>

      <Text style={styles.body} numberOfLines={2}>
        {notification.body}
      </Text>

      <View style={styles.footer}>
        <Text style={styles.time}>{formatDate(notification.sentAt)}</Text>
        {onDelete && (
          <TouchableOpacity onPress={() => onDelete(notification.id)}>
            <Icon name="trash-outline" size={18} color="#ef4444" />
          </TouchableOpacity>
        )}
      </View>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 6,
    padding: 16,
    elevation: 2,
  },
  unreadCard: {
    backgroundColor: '#f0f9ff',
    borderLeftWidth: 4,
    borderLeftColor: '#007AFF',
  },
  header: {
    flexDirection: 'row',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  typeIcon: {
    width: 36,
    height: 36,
    borderRadius: 18,
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 12,
  },
  headerInfo: {
    flex: 1,
  },
  title: {
    fontSize: 15,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 2,
  },
  category: {
    fontSize: 11,
    color: '#888',
  },
  unreadDot: {
    width: 8,
    height: 8,
    borderRadius: 4,
    backgroundColor: '#007AFF',
    marginLeft: 8,
  },
  body: {
    fontSize: 13,
    color: '#666',
    lineHeight: 18,
    marginBottom: 12,
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  time: {
    fontSize: 11,
    color: '#aaa',
  },
});