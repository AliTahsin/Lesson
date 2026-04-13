import React, { useState } from 'react';
import {
  View,
  Text,
  Image,
  TouchableOpacity,
  StyleSheet,
  Modal,
  TextInput
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { MenuItem } from '../types/restaurant';

interface Props {
  item: MenuItem;
  onAddToCart: (item: MenuItem, quantity: number, instructions: string) => void;
}

export const MenuItemCard: React.FC<Props> = ({ item, onAddToCart }) => {
  const [modalVisible, setModalVisible] = useState(false);
  const [quantity, setQuantity] = useState(1);
  const [instructions, setInstructions] = useState('');

  const handleAddToCart = () => {
    onAddToCart(item, quantity, instructions);
    setModalVisible(false);
    setQuantity(1);
    setInstructions('');
  };

  const getDietaryIcons = () => {
    const icons = [];
    if (item.isVegetarian) icons.push('🌱');
    if (item.isVegan) icons.push('🌿');
    if (item.isGlutenFree) icons.push('🚫🌾');
    return icons.join(' ');
  };

  return (
    <>
      <TouchableOpacity style={styles.card} onPress={() => setModalVisible(true)}>
        <Image
          source={{ uri: item.imageUrl || 'https://picsum.photos/200/150?random=' + item.id }}
          style={styles.image}
        />
        <View style={styles.content}>
          <View style={styles.header}>
            <Text style={styles.name}>{item.name}</Text>
            <Text style={styles.price}>€{item.price.toFixed(2)}</Text>
          </View>
          <Text style={styles.description} numberOfLines={2}>{item.description}</Text>
          <View style={styles.footer}>
            <Text style={styles.dietary}>{getDietaryIcons()}</Text>
            <Text style={styles.time}>⏱️ {item.preparationTimeMinutes} dk</Text>
          </View>
        </View>
      </TouchableOpacity>

      <Modal visible={modalVisible} transparent animationType="slide">
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>{item.name}</Text>
            <Text style={styles.modalPrice}>€{item.price.toFixed(2)}</Text>
            <Text style={styles.modalDescription}>{item.description}</Text>
            
            <View style={styles.dietaryInfo}>
              {item.isVegetarian && <Text style={styles.dietaryBadge}>🌱 Vejetaryen</Text>}
              {item.isVegan && <Text style={styles.dietaryBadge}>🌿 Vegan</Text>}
              {item.isGlutenFree && <Text style={styles.dietaryBadge}>🚫 Glutensiz</Text>}
            </View>

            <Text style={styles.sectionTitle}>Malzemeler</Text>
            <Text style={styles.ingredients}>{item.ingredients.join(', ')}</Text>

            <Text style={styles.sectionTitle}>Adet</Text>
            <View style={styles.quantityContainer}>
              <TouchableOpacity
                style={styles.quantityButton}
                onPress={() => setQuantity(Math.max(1, quantity - 1))}
              >
                <Icon name="remove" size={20} color="#007AFF" />
              </TouchableOpacity>
              <Text style={styles.quantity}>{quantity}</Text>
              <TouchableOpacity
                style={styles.quantityButton}
                onPress={() => setQuantity(quantity + 1)}
              >
                <Icon name="add" size={20} color="#007AFF" />
              </TouchableOpacity>
            </View>

            <Text style={styles.sectionTitle}>Özel İstekler</Text>
            <TextInput
              style={styles.instructionsInput}
              placeholder="Özel istekleriniz..."
              value={instructions}
              onChangeText={setInstructions}
              multiline
            />

            <View style={styles.modalButtons}>
              <TouchableOpacity
                style={[styles.modalButton, styles.cancelButton]}
                onPress={() => setModalVisible(false)}
              >
                <Text style={styles.cancelButtonText}>İptal</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.modalButton, styles.addButton]}
                onPress={handleAddToCart}
              >
                <Text style={styles.addButtonText}>Sepete Ekle</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </Modal>
    </>
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
    flexDirection: 'row',
  },
  image: {
    width: 100,
    height: 100,
  },
  content: {
    flex: 1,
    padding: 12,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 4,
  },
  name: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    flex: 1,
  },
  price: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  description: {
    fontSize: 12,
    color: '#666',
    marginBottom: 8,
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  dietary: {
    fontSize: 11,
    color: '#888',
  },
  time: {
    fontSize: 11,
    color: '#888',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'flex-end',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    backgroundColor: 'white',
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20,
    padding: 20,
    maxHeight: '80%',
  },
  modalTitle: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 4,
  },
  modalPrice: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#007AFF',
    marginBottom: 12,
  },
  modalDescription: {
    fontSize: 14,
    color: '#666',
    marginBottom: 12,
  },
  dietaryInfo: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    marginBottom: 12,
  },
  dietaryBadge: {
    backgroundColor: '#f0fdf4',
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
    marginRight: 8,
    fontSize: 12,
    color: '#16a34a',
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 12,
    marginBottom: 8,
  },
  ingredients: {
    fontSize: 13,
    color: '#666',
    marginBottom: 8,
  },
  quantityContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 16,
  },
  quantityButton: {
    width: 40,
    height: 40,
    borderRadius: 20,
    backgroundColor: '#f0f0f0',
    alignItems: 'center',
    justifyContent: 'center',
  },
  quantity: {
    fontSize: 18,
    fontWeight: 'bold',
    marginHorizontal: 20,
  },
  instructionsInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    padding: 12,
    fontSize: 14,
    minHeight: 60,
    textAlignVertical: 'top',
  },
  modalButtons: {
    flexDirection: 'row',
    marginTop: 20,
    gap: 12,
  },
  modalButton: {
    flex: 1,
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
  },
  cancelButton: {
    backgroundColor: '#f0f0f0',
  },
  addButton: {
    backgroundColor: '#007AFF',
  },
  cancelButtonText: {
    color: '#666',
    fontWeight: 'bold',
  },
  addButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});