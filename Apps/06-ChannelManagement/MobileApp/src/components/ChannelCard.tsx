import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import { Channel } from '../types/channel';

interface Props {
  channel: Channel;
  onPress: (id: number) => void;
  isConnected?: boolean;
  onConnect?: () => void;
}

export const ChannelCard: React.FC<Props> = ({ channel, onPress, isConnected, onConnect }) => {
  const getTypeColor = (type: string) => {
    switch (type) {
      case 'OTA': return '#3b82f6';
      case 'GDS': return '#8b5cf6';
      case 'Meta': return '#ec4899';
      case 'Direct': return '#10b981';
      default: return '#6b7280';
    }
  };

  return (
    <TouchableOpacity onPress={() => onPress(channel.id)} style={styles.card}>
      <View style={styles.header}>
        <View style={[styles.typeBadge, { backgroundColor: getTypeColor(channel.type) }]}>
          <Text style={styles.typeText}>{channel.type}</Text>
        </View>
        {isConnected !== undefined && (
          <View style={[styles.connectionBadge, { backgroundColor: isConnected ? '#10b981' : '#9ca3af' }]}>
            <Text style={styles.connectionText}>{isConnected ? 'Bağlı' : 'Bağlı Değil'}</Text>
          </View>
        )}
      </View>
      
      <Text style={styles.name}>{channel.name}</Text>
      <Text style={styles.code}>{channel.code}</Text>
      <Text style={styles.description} numberOfLines={2}>{channel.description}</Text>
      
      <View style={styles.footer}>
        <View style={styles.commissionContainer}>
          <Text style={styles.commissionLabel}>Komisyon</Text>
          <Text style={styles.commissionValue}>%{(channel.commission * 100).toFixed(0)}</Text>
        </View>
        {!isConnected && onConnect && (
          <TouchableOpacity style={styles.connectButton} onPress={onConnect}>
            <Text style={styles.connectButtonText}>Bağlan</Text>
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
    marginVertical: 8,
    padding: 16,
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 2,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 12,
  },
  typeBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  typeText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  connectionBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  connectionText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  name: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 4,
  },
  code: {
    fontSize: 12,
    color: '#888',
    marginBottom: 8,
  },
  description: {
    fontSize: 13,
    color: '#666',
    marginBottom: 12,
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: 8,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  commissionContainer: {
    flexDirection: 'row',
    alignItems: 'baseline',
  },
  commissionLabel: {
    fontSize: 12,
    color: '#888',
    marginRight: 4,
  },
  commissionValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#f59e0b',
  },
  connectButton: {
    backgroundColor: '#007AFF',
    paddingHorizontal: 16,
    paddingVertical: 6,
    borderRadius: 8,
  },
  connectButtonText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
});