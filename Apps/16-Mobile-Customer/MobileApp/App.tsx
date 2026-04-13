import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { LanguageProvider, useLanguageContext } from './src/context/LanguageContext';
import { HomeScreen } from './src/screens/HomeScreen';
import { ProfileScreen } from './src/screens/ProfileScreen';
import { DigitalKeyScreen } from './src/screens/DigitalKeyScreen';
import { RoomServiceScreen } from './src/screens/RoomServiceScreen';
import { RoomServiceDetailScreen } from './src/screens/RoomServiceDetailScreen';
import { SpaScreen } from './src/screens/SpaScreen';
import { SpaDetailScreen } from './src/screens/SpaDetailScreen';
import { SettingsScreen } from './src/screens/SettingsScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function HomeStack() {
  const { t } = useLanguageContext();
  return (
    <Stack.Navigator>
      <Stack.Screen name="HomeMain" component={HomeScreen} options={{ title: t('home') }} />
    </Stack.Navigator>
  );
}

function DigitalKeyStack() {
  const { t } = useLanguageContext();
  return (
    <Stack.Navigator>
      <Stack.Screen name="DigitalKeyMain" component={DigitalKeyScreen} options={{ title: t('digital_key') }} />
    </Stack.Navigator>
  );
}

function RoomServiceStack() {
  const { t } = useLanguageContext();
  return (
    <Stack.Navigator>
      <Stack.Screen name="RoomServiceMain" component={RoomServiceScreen} options={{ title: t('room_service') }} />
      <Stack.Screen name="RoomServiceDetail" component={RoomServiceDetailScreen} options={{ title: t('cart') }} />
    </Stack.Navigator>
  );
}

function SpaStack() {
  const { t } = useLanguageContext();
  return (
    <Stack.Navigator>
      <Stack.Screen name="SpaMain" component={SpaScreen} options={{ title: t('spa') }} />
      <Stack.Screen name="SpaDetail" component={SpaDetailScreen} options={{ title: t('book_appointment') }} />
    </Stack.Navigator>
  );
}

function ProfileStack() {
  const { t } = useLanguageContext();
  return (
    <Stack.Navigator>
      <Stack.Screen name="ProfileMain" component={ProfileScreen} options={{ title: t('profile') }} />
      <Stack.Screen name="Settings" component={SettingsScreen} options={{ title: t('settings') }} />
    </Stack.Navigator>
  );
}

function MainTabs() {
  const { t } = useLanguageContext();
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarIcon: ({ focused, color, size }) => {
          let iconName;
          if (route.name === 'Home') {
            iconName = focused ? 'home' : 'home-outline';
          } else if (route.name === 'DigitalKey') {
            iconName = focused ? 'key' : 'key-outline';
          } else if (route.name === 'RoomService') {
            iconName = focused ? 'restaurant' : 'restaurant-outline';
          } else if (route.name === 'Spa') {
            iconName = focused ? 'flower' : 'flower-outline';
          } else if (route.name === 'Profile') {
            iconName = focused ? 'person' : 'person-outline';
          }
          return <Icon name={iconName!} size={size} color={color} />;
        },
        tabBarActiveTintColor: '#007AFF',
        tabBarInactiveTintColor: 'gray',
      })}
    >
      <Tab.Screen name="Home" component={HomeStack} options={{ headerShown: false, title: t('home') }} />
      <Tab.Screen name="DigitalKey" component={DigitalKeyStack} options={{ headerShown: false, title: t('digital_key') }} />
      <Tab.Screen name="RoomService" component={RoomServiceStack} options={{ headerShown: false, title: t('room_service') }} />
      <Tab.Screen name="Spa" component={SpaStack} options={{ headerShown: false, title: t('spa') }} />
      <Tab.Screen name="Profile" component={ProfileStack} options={{ headerShown: false, title: t('profile') }} />
    </Tab.Navigator>
  );
}

export default function App() {
  return (
    <LanguageProvider>
      <NavigationContainer>
        <MainTabs />
      </NavigationContainer>
    </LanguageProvider>
  );
}