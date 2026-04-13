import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { TaskListScreen } from './src/screens/TaskListScreen';
import { TaskDetailScreen } from './src/screens/TaskDetailScreen';
import { IssueListScreen } from './src/screens/IssueListScreen';
import { HousekeepingDashboardScreen } from './src/screens/HousekeepingDashboardScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function TasksStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="TaskList" component={TaskListScreen} options={{ title: 'Görevler' }} />
      <Stack.Screen name="TaskDetail" component={TaskDetailScreen} options={{ title: 'Görev Detayı' }} />
    </Stack.Navigator>
  );
}

function IssuesStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="IssueList" component={IssueListScreen} options={{ title: 'Arızalar' }} />
    </Stack.Navigator>
  );
}

function DashboardStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Dashboard" component={HousekeepingDashboardScreen} options={{ title: 'Dashboard' }} />
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
            if (route.name === 'Tasks') {
              iconName = focused ? 'checkbox' : 'checkbox-outline';
            } else if (route.name === 'Issues') {
              iconName = focused ? 'warning' : 'warning-outline';
            } else if (route.name === 'Dashboard') {
              iconName = focused ? 'stats-chart' : 'stats-chart-outline';
            }
            return <Icon name={iconName!} size={size} color={color} />;
          },
          tabBarActiveTintColor: '#007AFF',
          tabBarInactiveTintColor: 'gray',
        })}
      >
        <Tab.Screen name="Tasks" component={TasksStack} options={{ headerShown: false }} />
        <Tab.Screen name="Issues" component={IssuesStack} options={{ headerShown: false }} />
        <Tab.Screen name="Dashboard" component={DashboardStack} options={{ headerShown: false }} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}