import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Image
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { restaurantApi } from '../services/restaurantApi';
import { Restaurant } from '../types/restaurant';

export const RestaurantListScreen = ({ navigation, route }: any) => {
  const { hotelId } = route.params;
  const [restaurants, setRestaurants] = useState<Restaurant[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadRestaurants();
  }, []);

  const loadRestaurants = async () => {
    setLoading(true);
    try {
      const data = await restaurantApi.getRestaurantsByHotel(hotelId);
      setRestaurants(data);
    } catch (error) {
      console.error('Error loading restaurants:', error);
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (restaurant: Restaurant) => {
    const now = new Date();
    const opening = restaurant.openingTime.split(':');
    const closing = restaurant.closingTime.split(':');
    const currentHour = now.getHours();
    const isOpen = currentHour >= parseInt(opening[0]) && currentHour < parseInt(closing[0]);
    return isOpen ? '#10b981' : '#ef4444';
  };

  const renderRestaurant = ({ item }: { item: Restaurant }) => (
    <TouchableOpacity
      style={styles.card}
      onPress={() => navigation.navigate('Menu', { restaurantId: item.id, restaurantName: item.name })}
    >
      <Image
        source={{ uri: item.images?.[0] || 'https://picsum.photos/400/200?random=' + item.id }}
        style={styles.image}
      />
      <View style={styles.content}>
        <View style={styles.header}>
          <Text style={styles.name}>{item.name}</Text>
          <View style={[styles.statusBadge, { backgroundColor: getStatusColor(item) }]}>
            <Text style={styles.statusText}>
              {getStatusColor(item) === '#10b981' ? 'Açık' : 'Kapalı'}
            </Text>
          </View>
        </View>
        <Text style={styles.cuisine}>🍽️ {item.cuisineType}</Text>
        <Text style={styles.description} numberOfLines={2}>{item.description}</Text>
        <View style={styles.infoRow}>
          <View style={styles.infoItem}>
            <Icon name="time-outline" size={14} color="#666" />
            <Text style={styles.infoText}>{item.openingTime} - {item.closingTime}</Text>
          </View>
          <View style={styles.infoItem}>
            <Icon name="people-outline" size={14} color="#666" />
            <Text style={styles.infoText}>{item.totalCapacity} kişi</Text>
          </View>
          <View style={styles.infoItem}>
            <Icon name="cash-outline" size={14} color="#666" />
            <Text style={styles.infoText}>€{item.averagePricePerPerson}</Text>
          </View>
        </View>
      </View>
    </TouchableOpacity>
  );

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <FlatList
        data={restaurants}
        keyExtractor={(item) => item.id.toString()}
        renderItem={renderRestaurant}
        contentContainerStyle={styles.listContent}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>Restoran bulunamadı</Text>
          </View>
        }
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
  listContent: {
    padding: 16,
  },
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginBottom: 16,
    overflow: 'hidden',
    elevation: 2,
  },
  image: {
    width: '100%',
    height: 150,
  },
  content: {
    padding: 16,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 8,
  },
  name: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
    flex: 1,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 11,
    fontWeight: 'bold',
  },
  cuisine: {
    fontSize: 14,
    color: '#666',
    marginBottom: 8,
  },
  description: {
    fontSize: 13,
    color: '#888',
    marginBottom: 12,
  },
  infoRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  infoItem: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  infoText: {
    fontSize: 11,
    color: '#666',
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