import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert,
  TextInput
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { staffApi } from '../services/staffApi';
import { StaffTask } from '../types/staff';
import { useStaffAuthContext } from '../context/StaffAuthContext';

export const TaskDetailScreen = ({ route, navigation }: any) => {
  const { taskId } = route.params;
  const { role } = useStaffAuthContext();
  const [task, setTask] = useState<StaffTask | null>(null);
  const [loading, setLoading] = useState(true);
  const [notes, setNotes] = useState('');
  const [completing, setCompleting] = useState(false);

  useEffect(() => {
    loadTask();
  }, []);

  const loadTask = async () => {
    setLoading(true);
    try {
      const data = await staffApi.getTaskById(taskId);
      setTask(data);
    } catch (error) {
      console.error('Error loading task:', error);
      Alert.alert('Hata', 'Görev detayı yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleStart = async () => {
    try {
      await staffApi.startTask(taskId);
      Alert.alert('Başarılı', 'Görev başlatıldı');
      loadTask();
    } catch (error) {
      Alert.alert('Hata', 'Görev başlatılamadı');
    }
  };

  const handleComplete = async () => {
    if (!notes) {
      Alert.alert('Uyarı', 'Lütfen tamamlama notu ekleyin');
      return;
    }

    setCompleting(true);
    try {
      await staffApi.completeTask(taskId, notes);
      Alert.alert('Başarılı', 'Görev tamamlandı');
      navigation.goBack();
    } catch (error) {
      Alert.alert('Hata', 'Görev tamamlanamadı');
    } finally {
      setCompleting(false);
    }
  };

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

  const formatDateTime = (dateString?: string) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  const canStart = task?.status === 'Assigned' && task.assignedToStaffId;
  const canComplete = task?.status === 'InProgress';

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!task) {
    return (
      <View style={styles.center}>
        <Text>Görev bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.taskNumber}>{task.taskNumber}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(task.status) }]}>
          <Text style={styles.statusText}>{task.status}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Oda Bilgileri</Text>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Oda No:</Text>
          <Text style={styles.infoValue}>{task.roomNumber}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Görev Tipi:</Text>
          <Text style={styles.infoValue}>{task.taskType}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Öncelik:</Text>
          <Text style={[styles.infoValue, { color: getPriorityColor(task.priority), fontWeight: 'bold' }]}>
            {task.priority}
          </Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Zaman Bilgileri</Text>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Oluşturulma:</Text>
          <Text style={styles.infoValue}>{formatDateTime(task.createdAt)}</Text>
        </View>
        {task.scheduledDate && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Planlanan:</Text>
            <Text style={styles.infoValue}>{formatDateTime(task.scheduledDate)}</Text>
          </View>
        )}
        {task.startedAt && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Başlangıç:</Text>
            <Text style={styles.infoValue}>{formatDateTime(task.startedAt)}</Text>
          </View>
        )}
        {task.completedAt && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Bitiş:</Text>
            <Text style={styles.infoValue}>{formatDateTime(task.completedAt)}</Text>
          </View>
        )}
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Açıklama</Text>
        <Text style={styles.description}>{task.description}</Text>
      </View>

      {task.notes && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Notlar</Text>
          <Text style={styles.notes}>{task.notes}</Text>
        </View>
      )}

      {canComplete && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Tamamlama Notu</Text>
          <TextInput
            style={styles.notesInput}
            placeholder="Görevle ilgili notlarınız..."
            value={notes}
            onChangeText={setNotes}
            multiline
            numberOfLines={3}
          />
        </View>
      )}

      {canStart && (
        <TouchableOpacity style={styles.startButton} onPress={handleStart}>
          <Text style={styles.startButtonText}>Göreve Başla</Text>
        </TouchableOpacity>
      )}

      {canComplete && (
        <TouchableOpacity
          style={[styles.completeButton, completing && styles.disabledButton]}
          onPress={handleComplete}
          disabled={completing}
        >
          <Text style={styles.completeButtonText}>
            {completing ? 'Tamamlanıyor...' : 'Görevi Tamamla'}
          </Text>
        </TouchableOpacity>
      )}
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
    padding: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  taskNumber: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  infoRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  infoLabel: {
    width: 100,
    fontSize: 14,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 14,
    color: '#333',
  },
  description: {
    fontSize: 14,
    color: '#555',
    lineHeight: 20,
  },
  notes: {
    fontSize: 14,
    color: '#666',
    fontStyle: 'italic',
  },
  notesInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    padding: 12,
    fontSize: 14,
    textAlignVertical: 'top',
    minHeight: 80,
  },
  startButton: {
    backgroundColor: '#3b82f6',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 14,
    alignItems: 'center',
  },
  startButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  completeButton: {
    backgroundColor: '#10b981',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 14,
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#9ca3af',
  },
  completeButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});