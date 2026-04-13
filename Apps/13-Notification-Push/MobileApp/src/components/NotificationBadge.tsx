import React, { useState, useEffect } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { notificationApi } from '../services/notificationApi';
import { useFocusEffect } from '@react-navigation/native';

interface Props {
  size?: 'small' | 'medium';
}

export const NotificationBadge: React.FC<Props> = ({ size = 'medium' }) => {
  const [count, setCount] = useState(0);

  const loadUnreadCount = async () => {
    try {
      const data = await notificationApi.getUnreadCount();
      setCount(data.count);
    } catch (error) {
      console.error('Error loading unread count:', error);
    }
  };

  useFocusEffect(
    React.useCallback(() => {
      loadUnreadCount();
    }, [])
  );

  if (count === 0) return null;

  const sizeStyles = size === 'small' ? styles.small : styles.medium;

  return (
    <View style={[styles.badge, sizeStyles]}>
      <Text style={[styles.text, size === 'small' && styles.smallText]}>
        {count > 99 ? '99+' : count}
      </Text>
    </View>
  );
};

const styles = StyleSheet.create({
  badge: {
    position: 'absolute',
    top: -5,
    right: -10,
    backgroundColor: '#ef4444',
    borderRadius: 12,
    justifyContent: 'center',
    alignItems: 'center',
  },
  small: {
    minWidth: 16,
    height: 16,
    borderRadius: 8,
  },
  medium: {
    minWidth: 20,
    height: 20,
    borderRadius: 10,
  },
  text: {
    color: 'white',
    fontWeight: 'bold',
    fontSize: 10,
  },
  smallText: {
    fontSize: 8,
  },
});