import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { MyReservationsScreen } from './src/screens/MyReservationsScreen';
import { ReservationDetailScreen } from './src/screens/ReservationDetailScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen 
          name="MyReservations" 
          component={MyReservationsScreen} 
          options={{ title: 'Rezervasyonlarım' }}
        />
        <Stack.Screen 
          name="ReservationDetail" 
          component={ReservationDetailScreen} 
          options={{ title: 'Rezervasyon Detayı' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}