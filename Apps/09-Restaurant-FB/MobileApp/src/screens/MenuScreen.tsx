import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { restaurantApi } from '../services/restaurantApi';
import { MenuItemCard } from '../components/MenuItemCard';
import { MenuItem, CartItem } from '../types/restaurant';

export const MenuScreen = ({ navigation, route }: any) => {
  const { restaurantId, restaurantName } = route.params;
  const [menus, setMenus] = useState<any[]>([]);
  const [selectedMenuId, setSelectedMenuId] = useState<number | null>(null);
  const [menuItems, setMenuItems] = useState<MenuItem[]>([]);
  const [cart, setCart] = useState<CartItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [categories, setCategories] = useState<string[]>([]);
  const [selectedCategory, setSelectedCategory] = useState<string>('all');

  useEffect(() => {
    loadMenus();
  }, []);

  useEffect(() => {
    if (selectedMenuId) {
      loadMenuItems(selectedMenuId);
    }
  }, [selectedMenuId]);

  const loadMenus = async () => {
    setLoading(true);
    try {
      const data = await restaurantApi.getMenusByRestaurant(restaurantId);
      setMenus(data);
      if (data.length > 0) {
        setSelectedMenuId(data[0].id);
      }
    } catch (error) {
      console.error('Error loading menus:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadMenuItems = async (menuId: number) => {
    setLoading(true);
    try {
      const items = await restaurantApi.getMenuItems(menuId);
      setMenuItems(items);
      const uniqueCategories = [...new Set(items.map(i => i.category))];
      setCategories(['all', ...uniqueCategories]);
    } catch (error) {
      console.error('Error loading menu items:', error);
    } finally {
      setLoading(false);
    }
  };

  const addToCart = (item: MenuItem, quantity: number, instructions: string) => {
    setCart(prevCart => {
      const existingIndex = prevCart.findIndex(c => c.menuItemId === item.id);
      if (existingIndex >= 0) {
        const updated = [...prevCart];
        updated[existingIndex].quantity += quantity;
        updated[existingIndex].totalPrice = updated[existingIndex].quantity * item.price;
        return updated;
      } else {
        return [...prevCart, {
          menuItemId: item.id,
          name: item.name,
          quantity,
          price: item.price,
          totalPrice: quantity * item.price,
          specialInstructions: instructions
        }];
      }
    });
    Alert.alert('Başarılı', `${item.name} sepete eklendi`);
  };

  const filteredItems = selectedCategory === 'all'
    ? menuItems
    : menuItems.filter(i => i.category === selectedCategory);

  const cartItemCount = cart.reduce((sum, item) => sum + item.quantity, 0);
  const cartTotal = cart.reduce((sum, item) => sum + item.totalPrice, 0);

  if (loading && !menuItems.length) {
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
        data={menus}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <TouchableOpacity
            style={[styles.menuTab, selectedMenuId === item.id && styles.menuTabActive]}
            onPress={() => setSelectedMenuId(item.id)}
          >
            <Text style={[styles.menuTabText, selectedMenuId === item.id && styles.menuTabTextActive]}>
              {item.name}
            </Text>
          </TouchableOpacity>
        )}
        showsHorizontalScrollIndicator={false}
        style={styles.menuTabs}
      />

      <FlatList
        horizontal
        data={categories}
        keyExtractor={(item) => item}
        renderItem={({ item }) => (
          <TouchableOpacity
            style={[styles.categoryChip, selectedCategory === item && styles.categoryChipActive]}
            onPress={() => setSelectedCategory(item)}
          >
            <Text style={[styles.categoryText, selectedCategory === item && styles.categoryTextActive]}>
              {item === 'all' ? 'Tümü' : item}
            </Text>
          </TouchableOpacity>
        )}
        showsHorizontalScrollIndicator={false}
        style={styles.categoryList}
      />

      <FlatList
        data={filteredItems}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <MenuItemCard item={item} onAddToCart={addToCart} />
        )}
        contentContainerStyle={styles.listContent}
      />

      {cartItemCount > 0 && (
        <TouchableOpacity
          style={styles.cartButton}
          onPress={() => navigation.navigate('Cart', {
            cart,
            restaurantId,
            restaurantName,
            setCart
          })}
        >
          <View style={styles.cartBadge}>
            <Text style={styles.cartBadgeText}>{cartItemCount}</Text>
          </View>
          <Text style={styles.cartButtonText}>Sepeti Görüntüle</Text>
          <Text style={styles.cartTotal}>€{cartTotal.toFixed(2)}</Text>
        </TouchableOpacity>
      )}
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
  menuTabs: {
    backgroundColor: 'white',
    maxHeight: 50,
  },
  menuTab: {
    paddingHorizontal: 20,
    paddingVertical: 12,
    borderBottomWidth: 2,
    borderBottomColor: 'transparent',
  },
  menuTabActive: {
    borderBottomColor: '#007AFF',
  },
  menuTabText: {
    fontSize: 14,
    color: '#666',
  },
  menuTabTextActive: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
  categoryList: {
    maxHeight: 40,
    marginVertical: 8,
  },
  categoryChip: {
    paddingHorizontal: 16,
    paddingVertical: 6,
    marginHorizontal: 4,
    backgroundColor: '#e0e0e0',
    borderRadius: 20,
  },
  categoryChipActive: {
    backgroundColor: '#007AFF',
  },
  categoryText: {
    fontSize: 12,
    color: '#333',
  },
  categoryTextActive: {
    color: 'white',
  },
  listContent: {
    paddingBottom: 80,
  },
  cartButton: {
    position: 'absolute',
    bottom: 20,
    left: 20,
    right: 20,
    backgroundColor: '#007AFF',
    borderRadius: 12,
    paddingVertical: 14,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingHorizontal: 20,
    elevation: 5,
  },
  cartBadge: {
    backgroundColor: '#ff3b30',
    borderRadius: 12,
    paddingHorizontal: 8,
    paddingVertical: 2,
  },
  cartBadgeText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
  cartButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  cartTotal: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});