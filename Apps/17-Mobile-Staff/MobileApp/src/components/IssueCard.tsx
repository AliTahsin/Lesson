import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { MaintenanceIssue } from '../types/staff';

interface Props {
  issue: MaintenanceIssue;
  onPress: (id: number) => void;
  onAction?: (id: number, action: string) => void;
  showActions?: boolean;
}

export const IssueCard: React.FC<Props> = ({ issue, onPress, onAction, showActions = false }) => {
  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'Critical': return '#ef4444';
      case 'High': return '#f59e0b';
      case 'Medium': return '#3b82f6';
      case 'Low': return '#10b981';
      default: return '#6b7280';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Reported': return '#f59e0b';
      case 'Assigned': return '#3b82f6';
      case 'InProgress': return '#8b5cf6';
      case 'Resolved': return '#10b981';
      case 'Closed': return '#6b7280';
      default: return '#6b7280';
    }
  };

  const getCategoryIcon = (category: string) => {
    switch (category) {
      case 'Plumbing': return '🚰';
      case 'Electrical': return '⚡';
      case 'HVAC': return '❄️';
      case 'Furniture': return '🪑';
      case 'Appliance': return '📺';
      default: return '🔧';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  const canStart = issue.status === 'Assigned' && issue.assignedToStaffName;
  const canResolve = issue.status === 'InProgress';

  return (
    <TouchableOpacity onPress={() => onPress(issue.id)} style={styles.card}>
      <View style={styles.header}>
        <View style={styles.categoryInfo}>
          <Text style={styles.categoryIcon}>{getCategoryIcon(issue.category)}</Text>
          <Text style={styles.category}>{issue.category}</Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(issue.status) }]}>
          <Text style={styles.statusText}>{issue.status}</Text>
        </View>
      </View>

      <View style={styles.roomInfo}>
        <Text style={styles.roomNumber}>Oda {issue.roomNumber}</Text>
        <Text style={[styles.priority, { color: getPriorityColor(issue.priority) }]}>
          {issue.priority}
        </Text>
      </View>

      <Text style={styles.description} numberOfLines={2}>
        {issue.description}
      </Text>

      <View style={styles.footer}>
        <Text style={styles.reportedBy}>📢 {issue.reportedByName}</Text>
        <Text style={styles.date}>{formatDate(issue.reportedAt)}</Text>
      </View>

      {showActions && (canStart || canResolve) && (
        <View style={styles.actions}>
          {canStart && (
            <TouchableOpacity
              style={[styles.actionButton, styles.startButton]}
              onPress={() => onAction?.(issue.id, 'start')}
            >
              <Text style={styles.actionButtonText}>Başla</Text>
            </TouchableOpacity>
          )}
          {canResolve && (
            <TouchableOpacity
              style={[styles.actionButton, styles.resolveButton]}
              onPress={() => onAction?.(issue.id, 'resolve')}
            >
              <Text style={styles.actionButtonText}>Çöz</Text>
            </TouchableOpacity>
          )}
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
    alignItems: 'center',
    marginBottom: 12,
  },
  categoryInfo: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 6,
  },
  categoryIcon: {
    fontSize: 16,
  },
  category: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
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
  roomInfo: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  roomNumber: {
    fontSize: 15,
    fontWeight: 'bold',
    color: '#333',
  },
  priority: {
    fontSize: 12,
    fontWeight: 'bold',
  },
  description: {
    fontSize: 13,
    color: '#666',
    marginBottom: 12,
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  reportedBy: {
    fontSize: 11,
    color: '#888',
  },
  date: {
    fontSize: 11,
    color: '#888',
  },
  actions: {
    flexDirection: 'row',
    justifyContent: 'flex-end',
    marginTop: 12,
    gap: 8,
  },
  actionButton: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 8,
  },
  startButton: {
    backgroundColor: '#3b82f6',
  },
  resolveButton: {
    backgroundColor: '#10b981',
  },
  actionButtonText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
});