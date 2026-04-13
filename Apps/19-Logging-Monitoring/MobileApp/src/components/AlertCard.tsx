import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { Alert } from '../types/logging';

interface Props {
  alert: Alert;
}

export const AlertCard: React.FC<Props> = ({ alert }) => {
  const getSeverityColor = (severity: string) => {
    switch (severity) {
      case 'Critical': return '#ef4444';
      case 'Warning': return '#f59e0b';
      case 'Info': return '#3b82f6';
      default: return '#6b7280';
    }
  };

  const getSeverityIcon = (severity: string) => {
    switch (severity) {
      case 'Critical': return 'alert-circle';
      case 'Warning': return 'warning';
      case 'Info': return 'information-circle';
      default: return 'help-circle';
    }
  };

  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  return (
    <View style={[styles.card, alert.isResolved && styles.resolvedCard]}>
      <View style={styles.header}>
        <View style={[styles.severityBadge, { backgroundColor: getSeverityColor(alert.severity) }]}>
          <Icon name={getSeverityIcon(alert.severity)} size={14} color="white" />
          <Text style={styles.severityText}>{alert.severity}</Text>
        </View>
        {alert.isResolved && (
          <View style={styles.resolvedBadge}>
            <Icon name="checkmark-circle" size={14} color="#10b981" />
            <Text style={styles.resolvedText}>Çözüldü</Text>
          </View>
        )}
      </View>

      <Text style={styles.name}>{alert.name}</Text>
      <Text style={styles.message}>{alert.message}</Text>

      <View style={styles.details}>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Servis:</Text>
          <Text style={styles.detailValue}>{alert.service}</Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Koşul:</Text>
          <Text style={styles.detailValue}>{alert.condition}</Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Değer:</Text>
          <Text style={[styles.detailValue, { color: getSeverityColor(alert.severity) }]}>
            {alert.currentValue} (Eşik: {alert.threshold})
          </Text>
        </View>
      </View>

      <Text style={styles.time}>
        {formatTime(alert.triggeredAt)}
      </Text>
    </View>
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
    borderLeftWidth: 4,
    borderLeftColor: '#ef4444',
  },
  resolvedCard: {
    borderLeftColor: '#10b981',
    opacity: 0.8,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  severityBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 12,
    gap: 4,
  },
  severityText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  resolvedBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  resolvedText: {
    fontSize: 11,
    color: '#10b981',
    fontWeight: 'bold',
  },
  name: {
    fontSize: 15,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 4,
  },
  message: {
    fontSize: 13,
    color: '#666',
    marginBottom: 12,
  },
  details: {
    marginBottom: 12,
  },
  detailRow: {
    flexDirection: 'row',
    marginBottom: 4,
  },
  detailLabel: {
    width: 50,
    fontSize: 12,
    color: '#888',
  },
  detailValue: {
    flex: 1,
    fontSize: 12,
    color: '#555',
  },
  time: {
    fontSize: 10,
    color: '#aaa',
    textAlign: 'right',
  },
});