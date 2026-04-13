import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TextInput
} from 'react-native';
import DatePicker from 'react-native-datepicker';
import Icon from 'react-native-vector-icons/Ionicons';
import { loggingApi } from '../services/loggingApi';
import { TraceCard } from '../components/TraceCard';
import { Trace } from '../types/logging';

export const TracesScreen = ({ navigation }: any) => {
  const [traces, setTraces] = useState<Trace[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [showSlowOnly, setShowSlowOnly] = useState(false);
  const [minDuration, setMinDuration] = useState('1000');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');

  useEffect(() => {
    loadTraces();
  }, [showSlowOnly, minDuration]);

  const loadTraces = async () => {
    setLoading(true);
    try {
      let data;
      if (showSlowOnly) {
        data = await loggingApi.getSlowTraces(parseInt(minDuration), startDate || undefined, endDate || undefined);
      } else {
        // Get traces from all services
        const services = ['HotelManagement', 'RoomManagement', 'ReservationSystem'];
        const allTraces: Trace[] = [];
        for (const service of services) {
          const serviceTraces = await loggingApi.getTracesByService(
            service,
            startDate || new Date(Date.now() - 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
            endDate || new Date().toISOString().split('T')[0]
          );
          allTraces.push(...serviceTraces);
        }
        data = allTraces;
      }
      setTraces(data);
    } catch (error) {
      console.error('Error loading traces:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadTraces();
  };

  const handleTracePress = (traceId: string) => {
    navigation.navigate('TraceDetail', { traceId });
  };

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.filterBar}>
        <TouchableOpacity
          style={[styles.slowButton, showSlowOnly && styles.slowButtonActive]}
          onPress={() => setShowSlowOnly(!showSlowOnly)}
        >
          <Text style={[styles.slowButtonText, showSlowOnly && styles.slowButtonTextActive]}>
            Sadece Yavaş İstekler
          </Text>
        </TouchableOpacity>
        
        {showSlowOnly && (
          <TextInput
            style={styles.durationInput}
            placeholder="Min süre (ms)"
            value={minDuration}
            onChangeText={setMinDuration}
            keyboardType="numeric"
          />
        )}
      </View>

      <FlatList
        data={traces}
        keyExtractor={(item) => item.traceId}
        renderItem={({ item }) => (
          <TraceCard trace={item} onPress={handleTracePress} />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="analytics-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Trace bulunamadı</Text>
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
  filterBar: {
    flexDirection: 'row',
    backgroundColor: 'white',
    padding: 12,
    alignItems: 'center',
    gap: 12,
  },
  slowButton: {
    flex: 1,
    paddingVertical: 8,
    alignItems: 'center',
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
  },
  slowButtonActive: {
    backgroundColor: '#007AFF',
  },
  slowButtonText: {
    color: '#666',
  },
  slowButtonTextActive: {
    color: 'white',
  },
  durationInput: {
    width: 100,
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 8,
    paddingVertical: 6,
    fontSize: 14,
  },
  listContent: {
    paddingBottom: 20,
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
});