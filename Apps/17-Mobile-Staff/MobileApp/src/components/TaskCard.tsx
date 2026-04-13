import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { StaffTask } from '../types/staff';

interface Props {
  task: StaffTask;
  onPress: (id: number) => void;
  onAction?: (id: number, action: string) => void;
  showActions?: boolean;
}

export const TaskCard: React.FC<Props> = ({ task, onPress, onAction, showActions = false }) => {
  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'High': return '#ef4444';
      case 'Medium': return '#f59e0b';
      case 'Low': return '#10b981';
      default: return '#6b7280';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Pending': return '#f59e0b';
      case 'Assigned': return '#3b82f6';
      case 'InProgress': return '#8b5cf6';
      case 'Completed': return '#10b981';
      default: return '#6b7280';
    }
  };

  const getTaskTypeIcon = (type: string) => {
    switch (type) {
      case 'CheckOut': return '🚪';
      case 'StayOver': return '🛏️';
      case 'DeepClean': return '✨';
      case 'Inspection': return '🔍';
      default: return '📋';
    }
  };

  const formatTime = (dateString?: string) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return `${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  const canStart = task.status === 'Assigned' && task.assignedToStaffId;
  const canComplete = task.status === 'InProgress';

  return (
    <TouchableOpacity onPress={() => onPress(task.id)} style={styles.card}>
      <View style={styles.header}>
        <View>
          <Text style={styles.roomNumber}>Oda {task.roomNumber}</Text>
          <Text style={styles.taskType}>
            {getTaskTypeIcon(task.taskType)} {task.taskType}
          </Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(task.status) }]}>
          <Text style={styles.statusText}>{task.status}</Text>
        </View>
      </View>

      <View style={styles.details}>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Öncelik:</Text>
          <Text style={[styles.priorityText, { color: getPriorityColor(task.priority) }]}>
            {task.priority}
          </Text>
        </View>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Oluşturulma:</Text>
          <Text style={styles.detailValue}>{formatTime(task.createdAt)}</Text>
        </View>
        {task.assignedToStaffName && (
          <View style={styles.detailRow}>
            <Text style={styles.detailLabel}>Atanan:</Text>
            <Text style={styles.detailValue}>{task.assignedToStaffName}</Text>
          </View>
        )}
      </View>

      <Text style={styles.description} numberOfLines={2}>
        {task.description}
      </Text>

      {showActions && (canStart || canComplete) && (
        <View style={styles.actions}>
          {canStart && (
            <TouchableOpacity
              style={[styles.actionButton, styles.startButton]}
              onPress={() => onAction?.(task.id, 'start')}
            >
              <Text style={styles.actionButtonText}>Başla</Text>
            </TouchableOpacity>
          )}
          {canComplete && (
            <TouchableOpacity
              style={[styles.actionButton, styles.completeButton]}
              onPress={() => onAction?.(task.id, 'complete')}
            >
              <Text style={styles.actionButtonText}>Tamamla</Text>
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
  roomNumber: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  taskType: {
    fontSize: 12,
    color: '#666',
    marginTop: 2,
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
  details: {
    marginBottom: 8,
  },
  detailRow: {
    flexDirection: 'row',
    marginBottom: 4,
  },
  detailLabel: {
    width: 80,
    fontSize: 12,
    color: '#888',
  },
  detailValue: {
    flex: 1,
    fontSize: 12,
    color: '#555',
  },
  priorityText: {
    fontSize: 12,
    fontWeight: 'bold',
  },
  description: {
    fontSize: 13,
    color: '#666',
    marginTop: 8,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
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
  completeButton: {
    backgroundColor: '#10b981',
  },
  actionButtonText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
});