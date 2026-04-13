import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  RefreshControl
} from 'react-native';
import { channelApi } from '../services/channelApi';
import { ChannelCard } from '../components/ChannelCard';
import { Channel, ChannelConnection } from '../types/channel';

export const ChannelListScreen = ({ navigation }: any) => {
  const [channels, setChannels] = useState<Channel[]>([]);
  const [connections, setConnections] = useState<ChannelConnection[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [selectedHotel, setSelectedHotel] = useState(1);
  const [stats, setStats] = useState<any>(null);

  const hotels = [
    { id: 1, name: 'Marriott Istanbul' },
    { id: 2, name: 'Hilton Izmir' },
    { id: 3, name: 'Sofitel Bodrum' }
  ];

  useEffect(() => {
    loadData();
  }, [selectedHotel]);

  const loadData = async () => {
    setLoading(true);
    try {
      const [channelsData, connectionsData, statsData] = await Promise.all([
        channelApi.getAllChannels(),
        channelApi.getHotelConnections(selectedHotel),
        channelApi.getDashboardStats()
      ]);
      setChannels(channelsData);
      setConnections(connectionsData);
      setStats(statsData);
    } catch (error) {
      console.error('Error loading data:', error);
      Alert.alert('Hata', 'Veriler yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadData();
  };

  const isChannelConnected = (channelId: number) => {
    return connections.some(c => c.channelId === channelId && c.connectionStatus === 'Active');
  };

  const handleConnect = async (channelId: number) => {
    Alert.alert(
      'Kanal Bağlantısı',
      'Bu kanalı otelinize bağlamak istediğinize emin misiniz?',
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Bağlan',
          onPress: async () => {
            try {
              await channelApi.connectChannel({
                channelId,
                hotelId: selectedHotel,
                autoSync: true
              });
              Alert.alert('Başarılı', 'Kanal başarıyla bağlandı');
              loadData();
            } catch (error) {
              Alert.alert('Hata', 'Bağlantı kurulamadı');
            }
          }
        }
      ]
    );
  };

  const handleDisconnect = async (channelId: number) => {
    const connection = connections.find(c => c.channelId === channelId);
    if (!connection) return;

    Alert.alert(
      'Kanal Bağlantısını Kes',
      'Bu kanal bağlantısını kaldırmak istediğinize emin misiniz?',
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Kaldır',
          onPress: async () => {
            try {
              await channelApi.disconnectChannel(connection.id);
              Alert.alert('Başarılı', 'Kanal bağlantısı kaldırıldı');
              loadData();
            } catch (error) {
              Alert.alert('Hata', 'Bağlantı kaldırılamadı');
            }
          }
        }
      ]
    );
  };

  const handleChannelPress = (channelId: number) => {
    navigation.navigate('ChannelDetail', { 
      channelId,
      hotelId: selectedHotel,
      isConnected: isChannelConnected(channelId)
    });
  };

  const getConnectionCount = () => {
    return connections.filter(c => c.connectionStatus === 'Active').length;
  };

  const getTotalRevenue = () => {
    return connections.reduce((sum, c) => sum + c.totalRevenue, 0);
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
      <View style={styles.hotelSelector}>
        <Text style={styles.hotelLabel}>Otel Seçin:</Text>
        <View style={styles.hotelButtons}>
          {hotels.map(hotel => (
            <TouchableOpacity
              key={hotel.id}
              style={[styles.hotelButton, selectedHotel === hotel.id && styles.hotelButtonActive]}
              onPress={() => setSelectedHotel(hotel.id)}
            >
              <Text style={[styles.hotelButtonText, selectedHotel === hotel.id && styles.hotelButtonTextActive]}>
                {hotel.name.split(' ')[0]}
              </Text>
            </TouchableOpacity>
          ))}
        </View>
      </View>

      {stats && (
        <View style={styles.statsCard}>
          <View style={styles.statsRow}>
            <View style={styles.statItem}>
              <Text style={styles.statValue}>{getConnectionCount()}</Text>
              <Text style={styles.statLabel}>Bağlı Kanal</Text>
            </View>
            <View style={styles.statItem}>
              <Text style={styles.statValue}>{connections.length}</Text>
              <Text style={styles.statLabel}>Toplam Bağlantı</Text>
            </View>
            <View style={styles.statItem}>
              <Text style={styles.statValue}>€{getTotalRevenue().toLocaleString()}</Text>
              <Text style={styles.statLabel}>Toplam Gelir</Text>
            </View>
          </View>
        </View>
      )}

      <FlatList
        data={channels}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <ChannelCard
            channel={item}
            onPress={handleChannelPress}
            isConnected={isChannelConnected(item.id)}
            onConnect={() => handleConnect(item.id)}
          />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
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
  hotelSelector: {
    backgroundColor: 'white',
    padding: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  hotelLabel: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
    marginBottom: 8,
  },
  hotelButtons: {
    flexDirection: 'row',
    gap: 8,
  },
  hotelButton: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 8,
    backgroundColor: '#f0f0f0',
  },
  hotelButtonActive: {
    backgroundColor: '#007AFF',
  },
  hotelButtonText: {
    color: '#333',
    fontSize: 14,
  },
  hotelButtonTextActive: {
    color: 'white',
  },
  statsCard: {
    backgroundColor: '#007AFF',
    margin: 16,
    borderRadius: 12,
    padding: 16,
  },
  statsRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
  },
  statItem: {
    alignItems: 'center',
  },
  statValue: {
    fontSize: 20,
    fontWeight: 'bold',
    color: 'white',
  },
  statLabel: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  listContent: {
    paddingBottom: 20,
  },
});