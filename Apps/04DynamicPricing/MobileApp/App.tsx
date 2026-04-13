import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { PriceCalculatorScreen } from './src/screens/PriceCalculatorScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen 
          name="PriceCalculator" 
          component={PriceCalculatorScreen} 
          options={{ title: 'Dinamik Fiyat Hesaplama' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}