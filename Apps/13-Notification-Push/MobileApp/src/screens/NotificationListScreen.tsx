import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { notificationApi } from '../services/notificationApi';
import { NotificationCard } from '../components/NotificationCard';
import { Notification } from '../types/notification';
import { usePushNotifications } from '../hooks/usePushNotifications';

export const NotificationListScreen = ({ navigation }: any) => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const { updateBadgeCount } = usePushNotifications();

  useEffect(() => {
    loadNotifications();
  }, []);

  const loadNotifications = async () => {
    setLoading(true);
    try {
      const data = await notificationApi.getUserNotifications();
      setNotifications(data);
      await updateBadgeCount();
    } catch (error) {
      console.error('Error loading notifications:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadNotifications();
  };

  const handleNotificationPress = async (id: number) => {
    const notification = notifications.find(n => n.id === id);
    if (notification && !notification.isRead) {
      await notificationApi.markAsRead(id);
      await updateBadgeCount();
      setNotifications(notifications.map(n =>
        n.id === id ? { ...n, isRead: true } : n
      ));
    }
    navigation.navigate('NotificationDetail', { notificationId: id });
  };

  const handleDelete = async (id: number) => {
    Alert.alert(
      'Bildirimi Sil',
      'Bu bildirimi silmek istediğinize emin misiniz?',
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Sil',
          style: 'destructive',
          onPress: async () => {
            await notificationApi.deleteNotification(id);
            setNotifications(notifications.filter(n => n.id !== id));
            await updateBadgeCount();
          }
        }
      ]
    );
  };

  const handleMarkAllAsRead = async () => {
    const unreadCount = notifications.filter(n => !n.isRead).length;
    if (unreadCount === 0) {
      Alert.alert('Bilgi', 'Tüm bildirimler zaten okunmuş');
      return;
    }

    Alert.alert(
      'Tümünü Okundu İşaretle',
      `${unreadCount} bildirimi okundu olarak işaretlemek istediğinize emin misiniz?`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'İşaretle',
          onPress: async () => {
            await notificationApi.markAllAsRead();
            setNotifications(notifications.map(n => ({ ...n, isRead: true })));
            await updateBadgeCount();
          }
        }
      ]
    );
  };

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {notifications.length > 0 && (
        <View style={styles.header}>
          <TouchableOpacity onPress={handleMarkAllAsRead}>
            <Text style={styles.markAllText}>Tümünü Okundu İşaretle</Text>
          </TouchableOpacity>
        </View>
      )}

      <FlatList
        data={notifications}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <NotificationCard
            notification={item}
            onPress={handleNotificationPress}
            onDelete={handleDelete}
          />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="notifications-off-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Henüz bildiriminiz yok</Text>
            <Text style={styles.emptySubtext}>
              Rezervasyon, ödeme ve kampanyalarla ilgili bildirimler burada görünecek
            </Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />
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
  header: {
    flexDirection: 'row',
    justifyContent: 'flex-end',
    paddingHorizontal: 16,
    paddingVertical: 12,
    backgroundColor: 'white',
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  markAllText: {
    color: '#007AFF',
    fontSize: 13,
    fontWeight: '500',
  },
  listContent: {
    paddingBottom: 20,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 100,
  },
  emptyText: {
    fontSize: 16,
    color: '#888',
    marginTop: 16,
  },
  emptySubtext: {
    fontSize: 13,
    color: '#aaa',
    textAlign: 'center',
    marginTop: 8,
    paddingHorizontal: 40,
  },
});