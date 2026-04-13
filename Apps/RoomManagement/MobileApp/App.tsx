import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { RoomListScreen } from './src/screens/RoomListScreen';
import { RoomDetailScreen } from './src/screens/RoomDetailScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen 
          name="RoomList" 
          component={RoomListScreen} 
          options={{ title: 'Odalar' }}
        />
        <Stack.Screen 
          name="RoomDetail" 
          component={RoomDetailScreen} 
          options={{ title: 'Oda Detayı' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}