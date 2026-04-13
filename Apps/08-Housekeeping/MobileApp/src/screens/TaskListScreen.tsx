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
import { taskApi } from '../services/taskApi';
import { signalRService } from '../services/signalRService';
import { TaskCard } from '../components/TaskCard';
import { HousekeepingTask } from '../types/housekeeping';

export const TaskListScreen = ({ navigation, route }: any) => {
  const { hotelId, staffId, isStaff = false } = route.params || {};
  const [tasks, setTasks] = useState<HousekeepingTask[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [filter, setFilter] = useState<string>('all');

  const filters = [
    { key: 'all', label: 'Tümü' },
    { key: 'Pending', label: 'Beklemede' },
    { key: 'Assigned', label: 'Atandı' },
    { key: 'InProgress', label: 'Devam Eden' },
    { key: 'Completed', label: 'Tamamlanan' }
  ];

  useEffect(() => {
    loadTasks();
    setupSignalR();
    return () => {
      signalRService.disconnect();
    };
  }, []);

  const setupSignalR = async () => {
    await signalRService.connect(hotelId, staffId);
    
    signalRService.on('NewTask', (task: HousekeepingTask) => {
      loadTasks();
    });
    
    signalRService.on('TaskAssigned', (task: HousekeepingTask) => {
      loadTasks();
    });
    
    signalRService.on('TaskUpdated', () => {
      loadTasks();
    });
  };

  const loadTasks = async () => {
    setLoading(true);
    try {
      let data;
      if (isStaff && staffId) {
        data = await taskApi.getTasksByStaff(staffId);
      } else if (hotelId) {
        data = await taskApi.getTasksByHotel(hotelId);
      } else {
        data = await taskApi.getAllTasks();
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
        await taskApi.startTask(taskId);
        Alert.alert('Başarılı', 'Görev başlatıldı');
      } else if (action === 'complete') {
        Alert.prompt('Görev Tamamlama', 'Not ekleyin:', async (notes) => {
          await taskApi.completeTask(taskId, notes || '');
          Alert.alert('Başarılı', 'Görev tamamlandı');
          loadTasks();
        });
      }
      loadTasks();
    } catch (error) {
      Alert.alert('Hata', 'İşlem başarısız');
    }
  };

  const filteredTasks = filter === 'all' 
    ? tasks 
    : tasks.filter(t => t.status === filter);

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <FlatList
        horizontal
        data={filters}
        keyExtractor={(item) => item.key}
        renderItem={({ item }) => (
          <TouchableOpacity
            style={[styles.filterChip, filter === item.key && styles.filterChipActive]}
            onPress={() => setFilter(item.key)}
          >
            <Text style={[styles.filterText, filter === item.key && styles.filterTextActive]}>
              {item.label}
            </Text>
          </TouchableOpacity>
        )}
        showsHorizontalScrollIndicator={false}
        style={styles.filterList}
      />

      <FlatList
        data={filteredTasks}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <TaskCard
            task={item}
            onPress={(id) => navigation.navigate('TaskDetail', { taskId: id, isStaff })}
            onAction={isStaff ? handleTaskAction : undefined}
            showActions={isStaff}
          />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>Görev bulunamadı</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />
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
  filterList: {
    maxHeight: 50,
    marginVertical: 8,
  },
  filterChip: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    marginHorizontal: 4,
    backgroundColor: '#e0e0e0',
    borderRadius: 20,
  },
  filterChipActive: {
    backgroundColor: '#007AFF',
  },
  filterText: {
    color: '#333',
    fontSize: 13,
  },
  filterTextActive: {
    color: 'white',
  },
  listContent: {
    paddingBottom: 20,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 50,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
  },
});