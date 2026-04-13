import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { DashboardScreen } from './src/screens/DashboardScreen';
import { ReportsScreen } from './src/screens/ReportsScreen';
import { RevenueReportScreen } from './src/screens/RevenueReportScreen';
import { OccupancyReportScreen } from './src/screens/OccupancyReportScreen';
import { ReservationReportScreen } from './src/screens/ReservationReportScreen';
import { CustomerReportScreen } from './src/screens/CustomerReportScreen';
import { ChannelReportScreen } from './src/screens/ChannelReportScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function ReportsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="ReportsMain" component={ReportsScreen} options={{ title: 'Raporlar' }} />
      <Stack.Screen name="RevenueReport" component={RevenueReportScreen} options={{ title: 'Gelir Raporu' }} />
      <Stack.Screen name="OccupancyReport" component={OccupancyReportScreen} options={{ title: 'Doluluk Raporu' }} />
      <Stack.Screen name="ReservationReport" component={ReservationReportScreen} options={{ title: 'Rezervasyon Raporu' }} />
      <Stack.Screen name="CustomerReport" component={CustomerReportScreen} options={{ title: 'Müşteri Raporu' }} />
      <Stack.Screen name="ChannelReport" component={ChannelReportScreen} options={{ title: 'Kanal Raporu' }} />
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
            } else if (route.name === 'Reports') {
              iconName = focused ? 'document-text' : 'document-text-outline';
            }
            return <Icon name={iconName!} size={size} color={color} />;
          },
          tabBarActiveTintColor: '#007AFF',
          tabBarInactiveTintColor: 'gray',
        })}
      >
        <Tab.Screen name="Dashboard" component={DashboardScreen} />
        <Tab.Screen name="Reports" component={ReportsStack} options={{ headerShown: false }} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}