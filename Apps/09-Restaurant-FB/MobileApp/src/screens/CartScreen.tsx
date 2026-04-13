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
import { CartItemComponent } from '../components/CartItem';
import { orderApi } from '../services/orderApi';
import { CartItem, CreateOrderDto } from '../types/restaurant';

export const CartScreen = ({ navigation, route }: any) => {
  const { cart, restaurantId, restaurantName, setCart, tableId, tableNumber, customerName: initialCustomerName } = route.params;
  const [customerName, setCustomerName] = useState(initialCustomerName || '');
  const [specialInstructions, setSpecialInstructions] = useState('');
  const [orderType, setOrderType] = useState('DineIn');
  const [loading, setLoading] = useState(false);

  const updateQuantity = (menuItemId: number, quantity: number) => {
    const updatedCart = cart.map((item: CartItem) =>
      item.menuItemId === menuItemId
        ? { ...item, quantity, totalPrice: quantity * item.price }
        : item
    );
    setCart(updatedCart);
  };

  const removeItem = (menuItemId: number) => {
    const updatedCart = cart.filter((item: CartItem) => item.menuItemId !== menuItemId);
    setCart(updatedCart);
  };

  const subTotal = cart.reduce((sum: number, item: CartItem) => sum + item.totalPrice, 0);
  const taxAmount = subTotal * 0.1;
  const totalAmount = subTotal + taxAmount;

  const handlePlaceOrder = async () => {
    if (!customerName.trim()) {
      Alert.alert('Uyarı', 'Lütfen adınızı girin');
      return;
    }

    if (cart.length === 0) {
      Alert.alert('Uyarı', 'Sepetiniz boş');
      return;
    }

    setLoading(true);
    try {
      const orderData: CreateOrderDto = {
        restaurantId,
        tableId,
        tableNumber,
        customerName: customerName.trim(),
        orderType,
        specialInstructions,
        items: cart.map((item: CartItem) => ({
          menuItemId: item.menuItemId,
          quantity: item.quantity,
          specialInstructions: item.specialInstructions
        }))
      };

      const order = await orderApi.createOrder(orderData);
      setCart([]);
      navigation.replace('OrderTracking', { orderId: order.id, orderNumber: order.orderNumber });
    } catch (error: any) {
      Alert.alert('Hata', error.response?.data?.error || 'Sipariş oluşturulamadı');
    } finally {
      setLoading(false);
    }
  };

  return (
    <View style={styles.container}>
      <FlatList
        data={cart}
        keyExtractor={(item) => item.menuItemId.toString()}
        renderItem={({ item }) => (
          <CartItemComponent
            item={item}
            onUpdateQuantity={updateQuantity}
            onRemove={removeItem}
          />
        )}
        ListHeaderComponent={
          <>
            <View style={styles.section}>
              <Text style={styles.sectionTitle}>Müşteri Bilgileri</Text>
              <TextInput
                style={styles.input}
                placeholder="Adınız Soyadınız"
                value={customerName}
                onChangeText={setCustomerName}
              />
            </View>

            <View style={styles.section}>
              <Text style={styles.sectionTitle}>Sipariş Tipi</Text>
              <View style={styles.orderTypeContainer}>
                <TouchableOpacity
                  style={[styles.orderTypeButton, orderType === 'DineIn' && styles.orderTypeActive]}
                  onPress={() => setOrderType('DineIn')}
                >
                  <Text style={[styles.orderTypeText, orderType === 'DineIn' && styles.orderTypeTextActive]}>
                    🍽️ Masa Siparişi
                  </Text>
                </TouchableOpacity>
                <TouchableOpacity
                  style={[styles.orderTypeButton, orderType === 'Takeaway' && styles.orderTypeActive]}
                  onPress={() => setOrderType('Takeaway')}
                >
                  <Text style={[styles.orderTypeText, orderType === 'Takeaway' && styles.orderTypeTextActive]}>
                    🥡 Paket Servis
                  </Text>
                </TouchableOpacity>
                <TouchableOpacity
                  style={[styles.orderTypeButton, orderType === 'RoomService' && styles.orderTypeActive]}
                  onPress={() => setOrderType('RoomService')}
                >
                  <Text style={[styles.orderTypeText, orderType === 'RoomService' && styles.orderTypeTextActive]}>
                    🏨 Oda Servisi
                  </Text>
                </TouchableOpacity>
              </View>
            </View>

            <View style={styles.section}>
              <Text style={styles.sectionTitle}>Genel Not</Text>
              <TextInput
                style={[styles.input, styles.textArea]}
                placeholder="Siparişinizle ilgili notlar..."
                value={specialInstructions}
                onChangeText={setSpecialInstructions}
                multiline
                numberOfLines={3}
              />
            </View>
          </>
        }
        ListFooterComponent={
          <View style={styles.summaryCard}>
            <Text style={styles.summaryTitle}>Sipariş Özeti</Text>
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
        }
        contentContainerStyle={styles.listContent}
      />

      <TouchableOpacity
        style={[styles.orderButton, loading && styles.disabledButton]}
        onPress={handlePlaceOrder}
        disabled={loading}
      >
        <Text style={styles.orderButtonText}>
          {loading ? 'Sipariş Oluşturuluyor...' : 'Sipariş Oluştur'}
        </Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  listContent: {
    paddingBottom: 80,
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
  input: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
  },
  textArea: {
    minHeight: 60,
    textAlignVertical: 'top',
  },
  orderTypeContainer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  orderTypeButton: {
    flex: 1,
    paddingVertical: 10,
    alignItems: 'center',
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
    marginHorizontal: 4,
  },
  orderTypeActive: {
    backgroundColor: '#007AFF',
  },
  orderTypeText: {
    fontSize: 12,
    color: '#333',
  },
  orderTypeTextActive: {
    color: 'white',
  },
  summaryCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  summaryTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
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
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  totalValue: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  orderButton: {
    position: 'absolute',
    bottom: 20,
    left: 20,
    right: 20,
    backgroundColor: '#16a34a',
    borderRadius: 12,
    paddingVertical: 14,
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