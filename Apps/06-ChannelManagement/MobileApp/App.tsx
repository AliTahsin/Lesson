import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { ChannelListScreen } from './src/screens/ChannelListScreen';
import { ChannelDetailScreen } from './src/screens/ChannelDetailScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator>
        <Stack.Screen 
          name="ChannelList" 
          component={ChannelListScreen} 
          options={{ title: 'Kanal Yönetimi' }}
        />
        <Stack.Screen 
          name="ChannelDetail" 
          component={ChannelDetailScreen} 
          options={{ title: 'Kanal Detayı' }}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
}