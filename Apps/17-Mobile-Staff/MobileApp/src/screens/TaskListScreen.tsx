import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { staffApi } from '../services/staffApi';
import { TaskCard } from '../components/TaskCard';
import { useStaffAuthContext } from '../context/StaffAuthContext';
import { StaffTask } from '../types/staff';

export const TaskListScreen = ({ navigation }: any) => {
  const { role, hotelId } = useStaffAuthContext();
  const [tasks, setTasks] = useState<StaffTask[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [filter, setFilter] = useState<string>('my');

  const isHousekeeping = role === 'Housekeeping' || role === 'Admin';

  useEffect(() => {
    loadTasks();
  }, [filter]);

  const loadTasks = async () => {
    setLoading(true);
    try {
      let data;
      if (filter === 'my') {
        data = await staffApi.getMyTasks();
      } else {
        data = await staffApi.getPendingTasks();
      }
      setTasks(data);
    } catch (error) {
      console.error('Error loading tasks:', error);
      Alert.alert('Hata', 'Görevler yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadTasks();
  };

  const handleTaskAction = async (taskId: number, action: string) => {
    try {
      if (action === 'start') {
        await staffApi.startTask(taskId);
        Alert.alert('Başarılı', 'Görev başlatıldı');
      } else if (action === 'complete') {
        Alert.prompt('Görev Tamamlama', 'Not ekleyin:', async (notes) => {
          await staffApi.completeTask(taskId, notes || '');
          Alert.alert('Başarılı', 'Görev tamamlandı');
          loadTasks();
        });
      }
      loadTasks();
    } catch (error) {
      Alert.alert('Hata', 'İşlem başarısız');
    }
  };

  const canManageTasks = role === 'Admin' || role === 'FrontDesk';

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {isHousekeeping && (
        <View style={styles.filterBar}>
          <TouchableOpacity
            style={[styles.filterButton, filter === 'my' && styles.filterButtonActive]}
            onPress={() => setFilter('my')}
          >
            <Text style={[styles.filterText, filter === 'my' && styles.filterTextActive]}>
              Görevlerim
            </Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.filterButton, filter === 'pending' && styles.filterButtonActive]}
            onPress={() => setFilter('pending')}
          >
            <Text style={[styles.filterText, filter === 'pending' && styles.filterTextActive]}>
              Bekleyen Görevler
            </Text>
          </TouchableOpacity>
        </View>
      )}

      <FlatList
        data={tasks}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <TaskCard
            task={item}
            onPress={(id) => navigation.navigate('TaskDetail', { taskId: id })}
            onAction={handleTaskAction}
            showActions={filter === 'my'}
          />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="checkbox-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Görev bulunamadı</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />

      {canManageTasks && (
        <TouchableOpacity
          style={styles.fab}
          onPress={() => navigation.navigate('CreateTask', { hotelId })}
        >
          <Icon name="add" size={28} color="white" />
        </TouchableOpacity>
      )}
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
  filterBar: {
    flexDirection: 'row',
    backgroundColor: 'white',
    padding: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  filterButton: {
    flex: 1,
    paddingVertical: 8,
    alignItems: 'center',
    borderRadius: 8,
  },
  filterButtonActive: {
    backgroundColor: '#007AFF',
  },
  filterText: {
    color: '#666',
    fontWeight: '500',
  },
  filterTextActive: {
    color: 'white',
  },
  listContent: {
    paddingBottom: 80,
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
  fab: {
    position: 'absolute',
    bottom: 20,
    right: 20,
    width: 56,
    height: 56,
    borderRadius: 28,
    backgroundColor: '#007AFF',
    alignItems: 'center',
    justifyContent: 'center',
    elevation: 5,
  },
});