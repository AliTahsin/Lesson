import React, { useState } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  Alert,
  TextInput
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { customerApi } from '../services/customerApi';
import { MenuItem } from '../types/customer';

export const RoomServiceDetailScreen = ({ route, navigation }: any) => {
  const { cart, setCart } = route.params;
  const [specialInstructions, setSpecialInstructions] = useState('');
  const [ordering, setOrdering] = useState(false);

  const updateQuantity = (itemId: number, delta: number) => {
    const newCart = [...cart];
    const index = newCart.findIndex(item => item.id === itemId);
    if (index !== -1) {
      const currentQty = (newCart[index] as any).quantity || 1;
      const newQuantity = currentQty + delta;
      if (newQuantity <= 0) {
        newCart.splice(index, 1);
      } else {
        newCart[index] = { ...newCart[index], quantity: newQuantity };
      }
      setCart(newCart);
    }
  };

  const getItemQuantity = (itemId: number) => {
    const item = cart.find((i: MenuItem) => i.id === itemId);
    return (item as any)?.quantity || 1;
  };

  const subTotal = cart.reduce((sum: number, item: MenuItem) => {
    const quantity = (item as any).quantity || 1;
    return sum + (item.price * quantity);
  }, 0);
  const taxAmount = subTotal * 0.1;
  const totalAmount = subTotal + taxAmount;

  const handlePlaceOrder = async () => {
    if (cart.length === 0) {
      Alert.alert('Uyarı', 'Sepetiniz boş');
      return;
    }

    setOrdering(true);
    try {
      const orderData = {
        items: cart.map((item: MenuItem) => ({
          menuItemId: item.id,
          itemName: item.name,
          quantity: (item as any).quantity || 1,
          unitPrice: item.price,
          specialInstructions
        })),
        specialInstructions
      };
      
      const order = await customerApi.createOrder(orderData);
      setCart([]);
      Alert.alert(
        'Başarılı',
        `Siparişiniz alındı. Sipariş No: ${order.orderNumber}`,
        [{ text: 'Tamam', onPress: () => navigation.goBack() }]
      );
    } catch (error) {
      Alert.alert('Hata', 'Sipariş oluşturulamadı');
    } finally {
      setOrdering(false);
    }
  };

  const renderCartItem = ({ item }: { item: MenuItem }) => {
    const quantity = getItemQuantity(item.id);
    return (
      <View style={styles.cartItem}>
        <View style={styles.itemInfo}>
          <Text style={styles.itemName}>{item.name}</Text>
          <Text style={styles.itemPrice}>€{item.price}</Text>
        </View>
        <View style={styles.quantityControls}>
          <TouchableOpacity
            style={styles.quantityButton}
            onPress={() => updateQuantity(item.id, -1)}
          >
            <Icon name="remove" size={16} color="#007AFF" />
          </TouchableOpacity>
          <Text style={styles.quantity}>{quantity}</Text>
          <TouchableOpacity
            style={styles.quantityButton}
            onPress={() => updateQuantity(item.id, 1)}
          >
            <Icon name="add" size={16} color="#007AFF" />
          </TouchableOpacity>
        </View>
        <Text style={styles.itemTotal}>€{(item.price * quantity).toFixed(2)}</Text>
      </View>
    );
  };

  if (cart.length === 0) {
    return (
      <View style={styles.emptyContainer}>
        <Icon name="cart-outline" size={64} color="#ccc" />
        <Text style={styles.emptyText}>Sepetiniz boş</Text>
        <TouchableOpacity
          style={styles.continueButton}
          onPress={() => navigation.goBack()}
        >
          <Text style={styles.continueButtonText}>Alışverişe Devam Et</Text>
        </TouchableOpacity>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <FlatList
        data={cart}
        keyExtractor={(item) => item.id.toString()}
        renderItem={renderCartItem}
        ListHeaderComponent={
          <View style={styles.section}>
            <Text style={styles.sectionTitle}>Sipariş Özeti</Text>
          </View>
        }
        ListFooterComponent={
          <>
            <View style={styles.section}>
              <Text style={styles.sectionTitle}>Özel İstekler</Text>
              <TextInput
                style={styles.textArea}
                placeholder="Özel istekleriniz varsa belirtin..."
                value={specialInstructions}
                onChangeText={setSpecialInstructions}
                multiline
                numberOfLines={3}
              />
            </View>

            <View style={styles.summaryCard}>
              <View style={styles.summaryRow}>
                <Text style={styles.summaryLabel}>Ara Toplam</Text>
                <Text style={styles.summaryValue}>€{subTotal.toFixed(2)}</Text>
              </View>
              <View style={styles.summaryRow}>
                <Text style={styles.summaryLabel}>KDV (%10)</Text>
                <Text style={styles.summaryValue}>€{taxAmount.toFixed(2)}</Text>
              </View>
              <View style={styles.divider} />
              <View style={styles.totalRow}>
                <Text style={styles.totalLabel}>Toplam</Text>
                <Text style={styles.totalValue}>€{totalAmount.toFixed(2)}</Text>
              </View>
            </View>

            <TouchableOpacity
              style={[styles.orderButton, ordering && styles.disabledButton]}
              onPress={handlePlaceOrder}
              disabled={ordering}
            >
              <Text style={styles.orderButtonText}>
                {ordering ? 'Sipariş Oluşturuluyor...' : 'Sipariş Ver'}
              </Text>
            </TouchableOpacity>
          </>
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
  listContent: {
    paddingBottom: 20,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  emptyText: {
    fontSize: 16,
    color: '#888',
    marginTop: 16,
  },
  continueButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingHorizontal: 20,
    paddingVertical: 10,
    marginTop: 16,
  },
  continueButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginTop: 12,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  cartItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: 'white',
    marginHorizontal: 16,
    marginTop: 8,
    padding: 12,
    borderRadius: 10,
  },
  itemInfo: {
    flex: 2,
  },
  itemName: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  itemPrice: {
    fontSize: 12,
    color: '#888',
    marginTop: 2,
  },
  quantityControls: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 10,
  },
  quantityButton: {
    width: 28,
    height: 28,
    borderRadius: 14,
    backgroundColor: '#f0f0f0',
    justifyContent: 'center',
    alignItems: 'center',
  },
  quantity: {
    fontSize: 14,
    fontWeight: 'bold',
    minWidth: 20,
    textAlign: 'center',
  },
  itemTotal: {
    width: 60,
    textAlign: 'right',
    fontSize: 14,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  textArea: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    padding: 12,
    fontSize: 14,
    minHeight: 80,
    textAlignVertical: 'top',
  },
  summaryCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginTop: 12,
    padding: 16,
  },
  summaryRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  summaryLabel: {
    fontSize: 14,
    color: '#666',
  },
  summaryValue: {
    fontSize: 14,
    color: '#333',
  },
  divider: {
    height: 1,
    backgroundColor: '#f0f0f0',
    marginVertical: 12,
  },
  totalRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  totalLabel: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  totalValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  orderButton: {
    backgroundColor: '#16a34a',
    borderRadius: 12,
    marginHorizontal: 16,
    marginTop: 16,
    marginBottom: 20,
    paddingVertical: 16,
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#9ca3af',
  },
  orderButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});