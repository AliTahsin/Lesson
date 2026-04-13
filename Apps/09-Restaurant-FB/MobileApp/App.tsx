import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { RestaurantListScreen } from './src/screens/RestaurantListScreen';
import { MenuScreen } from './src/screens/MenuScreen';
import { CartScreen } from './src/screens/CartScreen';
import { OrderTrackingScreen } from './src/screens/OrderTrackingScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen 
          name="RestaurantList" 
          component={RestaurantListScreen} 
          options={{ title: 'Restoranlar' }}
        />
        <Stack.Screen 
          name="Menu" 
          component={MenuScreen} 
          options={({ route }) => ({ title: route.params?.restaurantName || 'Menü' })}
        />
        <Stack.Screen 
          name="Cart" 
          component={CartScreen} 
          options={{ title: 'Sepetim' }}
        />
        <Stack.Screen 
          name="OrderTracking" 
          component={OrderTrackingScreen} 
          options={{ title: 'Sipariş Takibi' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}