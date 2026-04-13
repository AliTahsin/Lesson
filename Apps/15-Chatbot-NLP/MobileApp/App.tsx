import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createStackNavigator } from '@react-navigation/stack';
import { ChatScreen } from './src/screens/ChatScreen';
import { ChatHistoryScreen } from './src/screens/ChatHistoryScreen';

const Stack = createStackNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Stack.Navigator screenOptions={{ headerShown: false }}>
        <Stack.Screen name="Chat" component={ChatScreen} />
        <Stack.Screen name="ChatHistory" component={ChatHistoryScreen} />
      </Stack.Navigator>
    </NavigationContainer>
  );
}