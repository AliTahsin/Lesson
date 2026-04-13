import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { notificationApi } from '../services/notificationApi';
import { Notification } from '../types/notification';

export const NotificationDetailScreen = ({ route, navigation }: any) => {
  const { notificationId } = route.params;
  const [notification, setNotification] = useState<Notification | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadNotification();
  }, []);

  const loadNotification = async () => {
    setLoading(true);
    try {
      const data = await notificationApi.getNotificationById(notificationId);
      setNotification(data);
    } catch (error) {
      console.error('Error loading notification:', error);
      Alert.alert('Hata', 'Bildirim yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    Alert.alert(
      'Bildirimi Sil',
      'Bu bildirimi silmek istediğinize emin misiniz?',
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Sil',
          style: 'destructive',
          onPress: async () => {
            await notificationApi.deleteNotification(notificationId);
            navigation.goBack();
          }
        }
      ]
    );
  };

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

  const formatDateTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!notification) {
    return (
      <View style={styles.center}>
        <Text>Bildirim bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={[styles.headerCard, { borderTopColor: getTypeColor(notification.type) }]}>
        <View style={styles.headerIcon}>
          <Icon name={getTypeIcon(notification.type)} size={32} color={getTypeColor(notification.type)} />
        </View>
        <Text style={styles.title}>{notification.title}</Text>
        <Text style={styles.date}>{formatDateTime(notification.sentAt)}</Text>
      </View>

      <View style={styles.bodyCard}>
        <Text style={styles.body}>{notification.body}</Text>
      </View>

      {notification.category && (
        <View style={styles.infoCard}>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Kategori:</Text>
            <Text style={styles.infoValue}>{notification.category}</Text>
          </View>
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Tip:</Text>
            <Text style={[styles.infoValue, { color: getTypeColor(notification.type) }]}>
              {notification.type}
            </Text>
          </View>
          {notification.senderName && (
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>Gönderen:</Text>
              <Text style={styles.infoValue}>{notification.senderName}</Text>
            </View>
          )}
          {notification.relatedId && (
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>Referans No:</Text>
              <Text style={styles.infoValue}>#{notification.relatedId}</Text>
            </View>
          )}
        </View>
      )}

      <View style={styles.actionButtons}>
        {notification.actionUrl && (
          <TouchableOpacity
            style={styles.actionButton}
            onPress={() => {
              // Navigate to related screen
              navigation.goBack();
            }}
          >
            <Icon name="arrow-forward" size={18} color="#007AFF" />
            <Text style={styles.actionButtonText}>İlgili Sayfaya Git</Text>
          </TouchableOpacity>
        )}
        <TouchableOpacity
          style={[styles.actionButton, styles.deleteButton]}
          onPress={handleDelete}
        >
          <Icon name="trash-outline" size={18} color="#ef4444" />
          <Text style={[styles.actionButtonText, styles.deleteButtonText]}>
            Bildirimi Sil
          </Text>
        </TouchableOpacity>
      </View>
    </ScrollView>
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
    padding: 24,
    alignItems: 'center',
    borderTopWidth: 4,
  },
  headerIcon: {
    marginBottom: 16,
  },
  title: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
    textAlign: 'center',
    marginBottom: 8,
  },
  date: {
    fontSize: 12,
    color: '#888',
  },
  bodyCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 20,
  },
  body: {
    fontSize: 16,
    color: '#333',
    lineHeight: 24,
  },
  infoCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  infoRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  infoLabel: {
    width: 80,
    fontSize: 14,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 14,
    color: '#333',
  },
  actionButtons: {
    marginHorizontal: 16,
    marginBottom: 20,
    gap: 12,
  },
  actionButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    paddingVertical: 12,
    borderRadius: 8,
    backgroundColor: '#f0f0f0',
  },
  deleteButton: {
    backgroundColor: '#fee2e2',
  },
  actionButtonText: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
  deleteButtonText: {
    color: '#ef4444',
  },
});