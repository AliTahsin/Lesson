import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { PaymentScreen } from './src/screens/PaymentScreen';
import { PaymentHistoryScreen } from './src/screens/PaymentHistoryScreen';
import { InvoiceScreen } from './src/screens/InvoiceScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen 
          name="Payment" 
          component={PaymentScreen} 
          options={{ title: 'Ödeme Yap' }}
        />
        <Stack.Screen 
          name="PaymentHistory" 
          component={PaymentHistoryScreen} 
          options={{ title: 'Ödeme Geçmişi' }}
        />
        <Stack.Screen 
          name="Invoice" 
          component={InvoiceScreen} 
          options={{ title: 'Fatura' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}