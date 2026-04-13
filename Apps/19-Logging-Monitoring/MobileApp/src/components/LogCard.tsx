import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { LogEntry } from '../types/logging';
import { LevelBadge } from './LevelBadge';

interface Props {
  log: LogEntry;
  onPress: (id: string) => void;
}

export const LogCard: React.FC<Props> = ({ log, onPress }) => {
  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
  };

  const truncateMessage = (message: string, maxLength: number = 80) => {
    if (message.length <= maxLength) return message;
    return message.substring(0, maxLength) + '...';
  };

  return (
    <TouchableOpacity style={styles.card} onPress={() => onPress(log.id)}>
      <View style={styles.header}>
        <LevelBadge level={log.level} size="small" />
        <Text style={styles.time}>{formatTime(log.timestamp)}</Text>
      </View>

      <Text style={styles.message}>{truncateMessage(log.message)}</Text>

      <View style={styles.footer}>
        {log.sourceContext && (
          <View style={styles.footerItem}>
            <Icon name="server-outline" size={12} color="#888" />
            <Text style={styles.footerText}>{log.sourceContext.split('.').pop()}</Text>
          </View>
        )}
        {log.requestPath && (
          <View style={styles.footerItem}>
            <Icon name="link-outline" size={12} color="#888" />
            <Text style={styles.footerText}>{log.requestPath}</Text>
          </View>
        )}
        {log.durationMs && (
          <View style={styles.footerItem}>
            <Icon name="time-outline" size={12} color="#888" />
            <Text style={styles.footerText}>{log.durationMs}ms</Text>
          </View>
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
    padding: 12,
    elevation: 1,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  time: {
    fontSize: 11,
    color: '#888',
  },
  message: {
    fontSize: 13,
    color: '#333',
    marginBottom: 8,
    lineHeight: 18,
  },
  footer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 12,
    marginTop: 4,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  footerItem: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  footerText: {
    fontSize: 10,
    color: '#888',
  },
});