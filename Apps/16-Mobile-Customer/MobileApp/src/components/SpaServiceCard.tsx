import React from 'react';
import {
  View,
  Text,
  Image,
  TouchableOpacity,
  StyleSheet
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { SpaService } from '../types/customer';
import { useLanguageContext } from '../context/LanguageContext';

interface Props {
  service: SpaService;
  onPress: (service: SpaService) => void;
}

export const SpaServiceCard: React.FC<Props> = ({ service, onPress }) => {
  const { t } = useLanguageContext();

  return (
    <TouchableOpacity style={styles.card} onPress={() => onPress(service)}>
      <Image
        source={{ uri: service.imageUrl || `https://picsum.photos/200/150?random=${service.id}` }}
        style={styles.image}
      />
      <View style={styles.overlay}>
        <Text style={styles.category}>{service.category}</Text>
      </View>
      <View style={styles.content}>
        <Text style={styles.name}>{service.name}</Text>
        <Text style={styles.description} numberOfLines={2}>{service.description}</Text>
        <View style={styles.footer}>
          <View style={styles.infoRow}>
            <Icon name="time-outline" size={14} color="#666" />
            <Text style={styles.infoText}>{service.durationMinutes} {t('minutes')}</Text>
          </View>
          <View style={styles.infoRow}>
            <Icon name="cash-outline" size={14} color="#666" />
            <Text style={styles.price}>€{service.price}</Text>
          </View>
        </View>
      </View>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    overflow: 'hidden',
    elevation: 2,
  },
  image: {
    width: '100%',
    height: 140,
  },
  overlay: {
    position: 'absolute',
    top: 8,
    right: 8,
    backgroundColor: 'rgba(0,0,0,0.6)',
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 12,
  },
  category: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  content: {
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
  infoRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
  },
  infoText: {
    fontSize: 12,
    color: '#666',
  },
  price: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#007AFF',
  },
});