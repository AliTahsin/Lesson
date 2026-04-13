import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { LogDashboardScreen } from './src/screens/LogDashboardScreen';
import { LogListScreen } from './src/screens/LogListScreen';
import { LogDetailScreen } from './src/screens/LogDetailScreen';
import { MetricsScreen } from './src/screens/MetricsScreen';
import { TracesScreen } from './src/screens/TracesScreen';
import { TraceDetailScreen } from './src/screens/TraceDetailScreen';
import { AlertScreen } from './src/screens/AlertScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

// Logs Stack Navigator
function LogsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="LogDashboard" 
        component={LogDashboardScreen} 
        options={{ title: 'Dashboard' }} 
      />
      <Stack.Screen 
        name="LogList" 
        component={LogListScreen} 
        options={{ title: 'Loglar' }} 
      />
      <Stack.Screen 
        name="LogDetail" 
        component={LogDetailScreen} 
        options={{ title: 'Log Detayı' }} 
      />
    </Stack.Navigator>
  );
}

// Metrics Stack Navigator
function MetricsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="MetricsMain" 
        component={MetricsScreen} 
        options={{ title: 'Metrikler' }} 
      />
    </Stack.Navigator>
  );
}

// Traces Stack Navigator
function TracesStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="TracesMain" 
        component={TracesScreen} 
        options={{ title: 'Trace\'ler' }} 
      />
      <Stack.Screen 
        name="TraceDetail" 
        component={TraceDetailScreen} 
        options={{ title: 'Trace Detayı' }} 
      />
    </Stack.Navigator>
  );
}

// Alerts Stack Navigator
function AlertsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen 
        name="AlertsMain" 
        component={AlertScreen} 
        options={{ title: 'Alertler' }} 
      />
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
            if (route.name === 'Dashboard') {
              iconName = focused ? 'speedometer' : 'speedometer-outline';
            } else if (route.name === 'Logs') {
              iconName = focused ? 'document-text' : 'document-text-outline';
            } else if (route.name === 'Metrics') {
              iconName = focused ? 'stats-chart' : 'stats-chart-outline';
            } else if (route.name === 'Traces') {
              iconName = focused ? 'analytics' : 'analytics-outline';
            } else if (route.name === 'Alerts') {
              iconName = focused ? 'notifications' : 'notifications-outline';
            }
            return <Icon name={iconName!} size={size} color={color} />;
          },
          tabBarActiveTintColor: '#007AFF',
          tabBarInactiveTintColor: 'gray',
        })}
      >
        <Tab.Screen 
          name="Dashboard" 
          component={LogsStack} 
          options={{ headerShown: false, title: 'Dashboard' }} 
        />
        <Tab.Screen 
          name="Logs" 
          component={LogListScreen} 
          options={{ title: 'Loglar' }} 
        />
        <Tab.Screen 
          name="Metrics" 
          component={MetricsStack} 
          options={{ headerShown: false, title: 'Metrikler' }} 
        />
        <Tab.Screen 
          name="Traces" 
          component={TracesStack} 
          options={{ headerShown: false, title: 'Trace\'ler' }} 
        />
        <Tab.Screen 
          name="Alerts" 
          component={AlertsStack} 
          options={{ headerShown: false, title: 'Alertler' }} 
        />
      </Tab.Navigator>
    </NavigationContainer>
  );
}