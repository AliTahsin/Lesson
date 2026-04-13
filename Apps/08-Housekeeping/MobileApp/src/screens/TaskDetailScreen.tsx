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
import { taskApi } from '../services/taskApi';
import { StatusBadge } from '../components/StatusBadge';
import { HousekeepingTask } from '../types/housekeeping';

export const TaskDetailScreen = ({ route }: any) => {
  const { taskId, isStaff } = route.params;
  const [task, setTask] = useState<HousekeepingTask | null>(null);
  const [loading, setLoading] = useState(true);
  const [completing, setCompleting] = useState(false);
  const [notes, setNotes] = useState('');

  useEffect(() => {
    loadTask();
  }, []);

  const loadTask = async () => {
    setLoading(true);
    try {
      const data = await taskApi.getTaskById(taskId);
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
      await taskApi.startTask(taskId);
      Alert.alert('Başarılı', 'Görev başlatıldı');
      loadTask();
    } catch (error) {
      Alert.alert('Hata', 'Görev başlatılamadı');
    }
  };

  const handleComplete = async () => {
    if (!notes && completing) {
      Alert.alert('Uyarı', 'Lütfen bir not ekleyin');
      return;
    }

    setCompleting(true);
    try {
      await taskApi.completeTask(taskId, notes);
      Alert.alert('Başarılı', 'Görev tamamlandı');
      loadTask();
      setCompleting(false);
      setNotes('');
    } catch (error) {
      Alert.alert('Hata', 'Görev tamamlanamadı');
      setCompleting(false);
    }
  };

  const formatDateTime = (dateString?: string) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'High': return '#ef4444';
      case 'Medium': return '#f59e0b';
      case 'Low': return '#10b981';
      default: return '#6b7280';
    }
  };

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

  const canStart = isStaff && task.status === 'Assigned';
  const canComplete = isStaff && task.status === 'InProgress';

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.taskNumber}>{task.taskNumber}</Text>
        <StatusBadge status={task.status} type="task" />
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
        <Text style={styles.sectionTitle}>Süre Bilgileri</Text>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Tahmini Süre:</Text>
          <Text style={styles.infoValue}>{task.estimatedMinutes} dakika</Text>
        </View>
        {task.actualMinutes > 0 && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Gerçek Süre:</Text>
            <Text style={styles.infoValue}>{task.actualMinutes} dakika</Text>
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
    width: 90,
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
  completeButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  disabledButton: {
    opacity: 0.6,
  },
});