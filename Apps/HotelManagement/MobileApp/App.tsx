import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { HotelListScreen } from './src/screens/HotelListScreen';
import { HotelDetailScreen } from './src/screens/HotelDetailScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen 
          name="HotelList" 
          component={HotelListScreen} 
          options={{ title: 'Oteller' }}
        />
        <Stack.Screen 
          name="HotelDetail" 
          component={HotelDetailScreen} 
          options={{ title: 'Otel Detayı' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}