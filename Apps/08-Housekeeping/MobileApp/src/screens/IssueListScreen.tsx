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
import { issueApi } from '../services/issueApi';
import { signalRService } from '../services/signalRService';
import { IssueCard } from '../components/IssueCard';
import { MaintenanceIssue } from '../types/housekeeping';

export const IssueListScreen = ({ route }: any) => {
  const { hotelId } = route.params;
  const [issues, setIssues] = useState<MaintenanceIssue[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [filter, setFilter] = useState<string>('all');

  const filters = [
    { key: 'all', label: 'Tümü' },
    { key: 'Reported', label: 'Bildirilen' },
    { key: 'Assigned', label: 'Atanan' },
    { key: 'InProgress', label: 'Devam Eden' },
    { key: 'Resolved', label: 'Çözülen' }
  ];

  useEffect(() => {
    loadIssues();
    setupSignalR();
    return () => {
      signalRService.disconnect();
    };
  }, []);

  const setupSignalR = async () => {
    await signalRService.connect(hotelId);
    
    signalRService.on('CriticalIssue', () => {
      loadIssues();
      Alert.alert('Kritik Arıza', 'Yeni kritik bir arıza bildirildi!');
    });
    
    signalRService.on('IssueUpdated', () => {
      loadIssues();
    });
  };

  const loadIssues = async () => {
    setLoading(true);
    try {
      const data = await issueApi.getIssuesByHotel(hotelId);
      setIssues(data);
    } catch (error) {
      console.error('Error loading issues:', error);
      Alert.alert('Hata', 'Arızalar yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadIssues();
  };

  const filteredIssues = filter === 'all'
    ? issues
    : issues.filter(i => i.status === filter);

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
        data={filteredIssues}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <IssueCard issue={item} onPress={() => {}} />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>Arıza bulunamadı</Text>
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
    backgroundColor: '#ef4444',
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