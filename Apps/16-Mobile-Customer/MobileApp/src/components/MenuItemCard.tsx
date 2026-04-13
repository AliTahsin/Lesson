import React from 'react';
import {
  View,
  Text,
  Image,
  TouchableOpacity,
  StyleSheet
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { MenuItem } from '../types/customer';
import { useLanguageContext } from '../context/LanguageContext';

interface Props {
  item: MenuItem;
  onAddToCart: (item: MenuItem) => void;
}

export const MenuItemCard: React.FC<Props> = ({ item, onAddToCart }) => {
  const { t } = useLanguageContext();

  return (
    <View style={styles.card}>
      <Image
        source={{ uri: item.imageUrl || `https://picsum.photos/200/150?random=${item.id}` }}
        style={styles.image}
      />
      <View style={styles.content}>
        <Text style={styles.name}>{item.name}</Text>
        <Text style={styles.description} numberOfLines={2}>{item.description}</Text>
        <View style={styles.footer}>
          <View>
            <Text style={styles.price}>
              {item.currency} {item.price.toFixed(2)}
            </Text>
            <Text style={styles.time}>
              ⏱️ {item.preparationTimeMinutes} {t('minutes')}
            </Text>
          </View>
          <TouchableOpacity
            style={[styles.addButton, !item.isAvailable && styles.disabledButton]}
            onPress={() => onAddToCart(item)}
            disabled={!item.isAvailable}
          >
            <Icon name="add" size={20} color="white" />
          </TouchableOpacity>
        </View>
        {!item.isAvailable && (
          <View style={styles.unavailableBadge}>
            <Text style={styles.unavailableText}>Stokta Yok</Text>
          </View>
        )}
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    overflow: 'hidden',
    flexDirection: 'row',
    elevation: 2,
  },
  image: {
    width: 100,
    height: 100,
  },
  content: {
    flex: 1,
    padding: 12,
  },
  name: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  description: {
    fontSize: 12,
    color: '#666',
    marginTop: 4,
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: 8,
  },
  price: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  time: {
    fontSize: 11,
    color: '#888',
  },
  addButton: {
    backgroundColor: '#007AFF',
    width: 32,
    height: 32,
    borderRadius: 16,
    justifyContent: 'center',
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#ccc',
  },
  unavailableBadge: {
    position: 'absolute',
    top: 8,
    right: 8,
    backgroundColor: '#ef4444',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  unavailableText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
});