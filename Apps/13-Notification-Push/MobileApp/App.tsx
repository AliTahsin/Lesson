import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { NotificationListScreen } from './src/screens/NotificationListScreen';
import { NotificationDetailScreen } from './src/screens/NotificationDetailScreen';
import { NotificationBadge } from './src/components/NotificationBadge';
import { usePushNotifications } from './src/hooks/usePushNotifications';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function NotificationsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="NotificationList" 
        component={NotificationListScreen} 
        options={{ title: 'Bildirimler' }}
      />
      <Stack.Screen 
        name="NotificationDetail" 
        component={NotificationDetailScreen} 
        options={{ title: 'Bildirim Detayı' }}
      />
    </Stack.Navigator>
  );
}

function HomeStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Home" component={HomeScreen} options={{ title: 'Ana Sayfa' }} />
    </Stack.Navigator>
  );
}

function HomeScreen() {
  return (
    <View style={{ flex: 1, justifyContent: 'center', alignItems: 'center', backgroundColor: '#f5f5f5' }}>
      <Text>Ana Sayfa</Text>
    </View>
  );
}

function AppNavigator() {
  usePushNotifications();

  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarIcon: ({ focused, color, size }) => {
          let iconName;
          if (route.name === 'Home') {
            iconName = focused ? 'home' : 'home-outline';
          } else if (route.name === 'Notifications') {
            iconName = focused ? 'notifications' : 'notifications-outline';
          }
          return <Icon name={iconName!} size={size} color={color} />;
        },
        tabBarActiveTintColor: '#007AFF',
        tabBarInactiveTintColor: 'gray',
      })}
    >
      <Tab.Screen name="Home" component={HomeStack} options={{ headerShown: false }} />
      <Tab.Screen 
        name="Notifications" 
        component={NotificationsStack} 
        options={{ 
          headerShown: false,
          tabBarBadge: () => <NotificationBadge />
        }}
      />
    </Tab.Navigator>
  );
}

export default function App() {
  return (
    <NavigationContainer>
      <AppNavigator />
    </NavigationContainer>
  );
}