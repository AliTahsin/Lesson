import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TouchableOpacity
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { staffApi } from '../services/staffApi';
import { signalRService } from '../services/signalRService';
import { NotificationCard } from '../components/NotificationCard';
import { Notification } from '../types/staff';
import { useStaffAuthContext } from '../context/StaffAuthContext';

export const NotificationScreen = ({ navigation }: any) => {
  const { staff, hotelId } = useStaffAuthContext();
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  useEffect(() => {
    loadNotifications();
    setupSignalR();
    return () => {
      signalRService.disconnect();
    };
  }, []);

  const loadNotifications = async () => {
    setLoading(true);
    try {
      const data = await staffApi.getNotifications();
      setNotifications(data);
    } catch (error) {
      console.error('Error loading notifications:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const setupSignalR = async () => {
    if (!hotelId || !staff?.id) return;
    
    await signalRService.connect(hotelId, staff.id);
    
    signalRService.on('NewNotification', (notification: Notification) => {
      setNotifications(prev => [notification, ...prev]);
    });
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadNotifications();
  };

  const handleNotificationPress = async (id: number) => {
    const notification = notifications.find(n => n.id === id);
    if (notification && !notification.isRead) {
      await staffApi.markAsRead(id);
      setNotifications(notifications.map(n =>
        n.id === id ? { ...n, isRead: true } : n
      ));
    }
    
    // Navigate based on notification type
    if (notification?.type === 'Task' && notification.relatedId) {
      navigation.navigate('TaskDetail', { taskId: notification.relatedId });
    } else if (notification?.type === 'Issue' && notification.relatedId) {
      navigation.navigate('IssueDetail', { issueId: notification.relatedId });
    }
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
      <FlatList
        data={notifications}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <NotificationCard
            notification={item}
            onPress={handleNotificationPress}
          />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="notifications-off-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Henüz bildiriminiz yok</Text>
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
    fontSize: 14,
    color: '#888',
  },
});