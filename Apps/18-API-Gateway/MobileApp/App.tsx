import React from 'react';
import { NavigationContainer } from '@react-navigation/navigation-container';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { GatewayProvider } from './src/context/GatewayContext';
import { GatewayStatusScreen } from './src/screens/GatewayStatusScreen';
import { ServiceListScreen } from './src/screens/ServiceListScreen';
import { ServiceDetailScreen } from './src/screens/ServiceDetailScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

// Status Stack (Dashboard ve Detay)
function StatusStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="GatewayStatusMain" 
        component={GatewayStatusScreen} 
        options={{ title: 'API Gateway Dashboard' }}
      />
      <Stack.Screen 
        name="ServiceDetail" 
        component={ServiceDetailScreen} 
        options={{ title: 'Servis Detayı' }}
      />
    </Stack.Navigator>
  );
}

// Services Stack (Liste ve Detay)
function ServicesStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="ServiceListMain" 
        component={ServiceListScreen} 
        options={{ title: 'Mikroservisler' }}
      />
      <Stack.Screen 
        name="ServiceDetail" 
        component={ServiceDetailScreen} 
        options={{ title: 'Servis Detayı' }}
      />
    </Stack.Navigator>
  );
}

function MainTabs() {
  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarIcon: ({ focused, color, size }) => {
          let iconName;
          if (route.name === 'Status') {
            iconName = focused ? 'speedometer' : 'speedometer-outline';
          } else if (route.name === 'Services') {
            iconName = focused ? 'server' : 'server-outline';
          }
          return <Icon name={iconName!} size={size} color={color} />;
        },
        tabBarActiveTintColor: '#007AFF',
        tabBarInactiveTintColor: 'gray',
        headerShown: false,
      })}
    >
      <Tab.Screen name="Status" component={StatusStack} />
      <Tab.Screen name="Services" component={ServicesStack} />
    </Tab.Navigator>
  );
}

export default function App() {
  return (
    <GatewayProvider autoRefresh={true}>
      <NavigationContainer>
        <MainTabs />
      </NavigationContainer>
    </GatewayProvider>
  );
}