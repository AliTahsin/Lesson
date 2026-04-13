import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { MeetingRoomListScreen } from './src/screens/MeetingRoomListScreen';
import { MeetingRoomDetailScreen } from './src/screens/MeetingRoomDetailScreen';
import { EventListScreen } from './src/screens/EventListScreen';
import { EventDetailScreen } from './src/screens/EventDetailScreen';
import { AttendeeCheckInScreen } from './src/screens/AttendeeCheckInScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function RoomsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="MeetingRoomList" component={MeetingRoomListScreen} options={{ title: 'Toplantı Odaları' }} />
      <Stack.Screen name="MeetingRoomDetail" component={MeetingRoomDetailScreen} options={{ title: 'Oda Detayı' }} />
    </Stack.Navigator>
  );
}

function EventsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="EventList" component={EventListScreen} options={{ title: 'Etkinlikler' }} />
      <Stack.Screen name="EventDetail" component={EventDetailScreen} options={{ title: 'Etkinlik Detayı' }} />
      <Stack.Screen name="AttendeeCheckIn" component={AttendeeCheckInScreen} options={{ title: 'Katılımcı Girişi' }} />
    </Stack.Navigator>
  );
}

export default function App() {
  return (
    <NavigationContainer>
      <Tab.Navigator
        screenOptions={({ route }) => ({
          tabBarIcon: ({ focused, color, size }) => {
            let iconName;
            if (route.name === 'Rooms') {
              iconName = focused ? 'business' : 'business-outline';
            } else if (route.name === 'Events') {
              iconName = focused ? 'calendar' : 'calendar-outline';
            }
            return <Icon name={iconName!} size={size} color={color} />;
          },
          tabBarActiveTintColor: '#007AFF',
          tabBarInactiveTintColor: 'gray',
        })}
      >
        <Tab.Screen name="Rooms" component={RoomsStack} options={{ headerShown: false }} />
        <Tab.Screen name="Events" component={EventsStack} options={{ headerShown: false }} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}