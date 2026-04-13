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
import { aiApi } from '../services/aiApi';
import { RecommendationCard } from '../components/RecommendationCard';
import { Recommendation, RecommendationMetrics } from '../types/ai';

export const RecommendationScreen = ({ navigation }: any) => {
  const [recommendations, setRecommendations] = useState<Recommendation[]>([]);
  const [metrics, setMetrics] = useState<RecommendationMetrics | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [selectedType, setSelectedType] = useState<string>('personalized');

  const loadRecommendations = async () => {
    setLoading(true);
    try {
      let data: Recommendation[];
      if (selectedType === 'personalized') {
        data = await aiApi.getPersonalizedRecommendations(20);
      } else if (selectedType === 'popular') {
        data = await aiApi.getPopularItems('Hotel', 20);
      } else {
        data = await aiApi.getSimilarItems(1, 'Hotel', 20);
      }
      setRecommendations(data);
      
      const metricsData = await aiApi.getRecommendationMetrics();
      setMetrics(metricsData);
    } catch (error) {
      console.error('Error loading recommendations:', error);
      Alert.alert('Hata', 'Öneriler yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    loadRecommendations();
  }, [selectedType]);

  const onRefresh = () => {
    setRefreshing(true);
    loadRecommendations();
  };

  const handleRecommendationPress = async (itemId: number, itemType: string) => {
    // Track click
    const recommendation = recommendations.find(r => r.itemId === itemId);
    if (recommendation) {
      await aiApi.trackClick(recommendation.id);
    }
    
    // Navigate to detail screen
    if (itemType === 'Hotel') {
      navigation.navigate('HotelDetail', { hotelId: itemId });
    } else if (itemType === 'Room') {
      navigation.navigate('RoomDetail', { roomId: itemId });
    }
  };

  const handleBooking = async (recommendationId: number) => {
    await aiApi.trackBooking(recommendationId);
    Alert.alert('Başarılı', 'Rezervasyon sayfasına yönlendiriliyorsunuz');
    loadRecommendations();
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
      <View style={styles.typeSelector}>
        <TouchableOpacity
          style={[styles.typeButton, selectedType === 'personalized' && styles.typeButtonActive]}
          onPress={() => setSelectedType('personalized')}
        >
          <Text style={[styles.typeText, selectedType === 'personalized' && styles.typeTextActive]}>
            Kişiselleştirilmiş
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.typeButton, selectedType === 'popular' && styles.typeButtonActive]}
          onPress={() => setSelectedType('popular')}
        >
          <Text style={[styles.typeText, selectedType === 'popular' && styles.typeTextActive]}>
            Popüler
          </Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.typeButton, selectedType === 'similar' && styles.typeButtonActive]}
          onPress={() => setSelectedType('similar')}
        >
          <Text style={[styles.typeText, selectedType === 'similar' && styles.typeTextActive]}>
            Benzer
          </Text>
        </TouchableOpacity>
      </View>

      {metrics && (
        <View style={styles.metricsCard}>
          <View style={styles.metricItem}>
            <Text style={styles.metricValue}>{metrics.clickThroughRate.toFixed(1)}%</Text>
            <Text style={styles.metricLabel}>Tıklanma Oranı</Text>
          </View>
          <View style={styles.metricDivider} />
          <View style={styles.metricItem}>
            <Text style={styles.metricValue}>{metrics.conversionRate.toFixed(1)}%</Text>
            <Text style={styles.metricLabel}>Dönüşüm Oranı</Text>
          </View>
          <View style={styles.metricDivider} />
          <View style={styles.metricItem}>
            <Text style={styles.metricValue}>{metrics.totalRecommendations}</Text>
            <Text style={styles.metricLabel}>Toplam Öneri</Text>
          </View>
        </View>
      )}

      <FlatList
        data={recommendations}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <RecommendationCard
            recommendation={item}
            onPress={handleRecommendationPress}
            onBook={handleBooking}
          />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="bulb-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Öneri bulunamadı</Text>
            <Text style={styles.emptySubtext}>
              Daha fazla otel ve oda inceleyerek kişiselleştirilmiş öneriler alın
            </Text>
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
  typeSelector: {
    flexDirection: 'row',
    backgroundColor: 'white',
    padding: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  typeButton: {
    flex: 1,
    paddingVertical: 8,
    alignItems: 'center',
    borderRadius: 8,
  },
  typeButtonActive: {
    backgroundColor: '#007AFF',
  },
  typeText: {
    color: '#666',
    fontWeight: '500',
  },
  typeTextActive: {
    color: 'white',
  },
  metricsCard: {
    flexDirection: 'row',
    backgroundColor: 'white',
    marginHorizontal: 16,
    marginTop: 12,
    marginBottom: 8,
    borderRadius: 12,
    padding: 16,
    justifyContent: 'space-around',
  },
  metricItem: {
    alignItems: 'center',
  },
  metricValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  metricLabel: {
    fontSize: 11,
    color: '#888',
    marginTop: 4,
  },
  metricDivider: {
    width: 1,
    backgroundColor: '#e0e0e0',
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
    fontSize: 16,
    color: '#888',
    marginTop: 16,
  },
  emptySubtext: {
    fontSize: 13,
    color: '#aaa',
    textAlign: 'center',
    marginTop: 8,
    paddingHorizontal: 40,
  },
});