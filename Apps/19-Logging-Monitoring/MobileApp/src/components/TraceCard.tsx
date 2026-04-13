import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { Trace } from '../types/logging';

interface Props {
  trace: Trace;
  onPress: (traceId: string) => void;
}

export const TraceCard: React.FC<Props> = ({ trace, onPress }) => {
  const getDurationColor = (durationMs: number) => {
    if (durationMs < 200) return '#10b981';
    if (durationMs < 1000) return '#f59e0b';
    return '#ef4444';
  };

  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
  };

  return (
    <TouchableOpacity style={styles.card} onPress={() => onPress(trace.traceId)}>
      <View style={styles.header}>
        <Text style={styles.operationName}>{trace.operationName}</Text>
        <Text style={[styles.duration, { color: getDurationColor(trace.durationMs) }]}>
          {trace.durationMs}ms
        </Text>
      </View>

      <View style={styles.details}>
        <View style={styles.detailRow}>
          <Icon name="server-outline" size={14} color="#666" />
          <Text style={styles.detailText}>{trace.service}</Text>
        </View>
        <View style={styles.detailRow}>
          <Icon name="link-outline" size={14} color="#666" />
          <Text style={styles.detailText}>{trace.endpoint}</Text>
        </View>
        <View style={styles.detailRow}>
          <Icon name="time-outline" size={14} color="#666" />
          <Text style={styles.detailText}>{formatTime(trace.startTime)}</Text>
        </View>
      </View>

      <View style={styles.tags}>
        {Object.entries(trace.tags).slice(0, 3).map(([key, value]) => (
          <View key={key} style={styles.tag}>
            <Text style={styles.tagText}>{key}: {value}</Text>
          </View>
        ))}
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
    elevation: 1,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  operationName: {
    fontSize: 15,
    fontWeight: 'bold',
    color: '#333',
  },
  duration: {
    fontSize: 14,
    fontWeight: 'bold',
  },
  details: {
    marginBottom: 12,
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
  tags: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 6,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  tag: {
    backgroundColor: '#f0f0f0',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
  },
  tagText: {
    fontSize: 10,
    color: '#666',
  },
});