import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { StaffAuthProvider, useStaffAuthContext } from './src/context/StaffAuthContext';
import { LoginScreen } from './src/screens/LoginScreen';
import { DashboardScreen } from './src/screens/DashboardScreen';
import { TaskListScreen } from './src/screens/TaskListScreen';
import { TaskDetailScreen } from './src/screens/TaskDetailScreen';
import { IssueListScreen } from './src/screens/IssueListScreen';
import { IssueDetailScreen } from './src/screens/IssueDetailScreen';
import { CheckInScreen } from './src/screens/CheckInScreen';
import { CheckOutScreen } from './src/screens/CheckOutScreen';
import { ProfileScreen } from './src/screens/ProfileScreen';
import { NotificationScreen } from './src/screens/NotificationScreen';
import { RoleBasedRoute } from './src/components/RoleBasedRoute';

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
      <Stack.Screen name="IssueDetail" component={IssueDetailScreen} options={{ title: 'Arıza Detayı' }} />
    </Stack.Navigator>
  );
}

function CheckStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="CheckIn" component={CheckInScreen} options={{ title: 'Check-in' }} />
      <Stack.Screen name="CheckOut" component={CheckOutScreen} options={{ title: 'Check-out' }} />
    </Stack.Navigator>
  );
}

function ProfileStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Profile" component={ProfileScreen} options={{ title: 'Profil' }} />
      <Stack.Screen name="Notifications" component={NotificationScreen} options={{ title: 'Bildirimler' }} />
    </Stack.Navigator>
  );
}

function MainTabs() {
  const { role } = useStaffAuthContext();
  const isFrontDesk = role === 'FrontDesk' || role === 'Admin';
  const isHousekeeping = role === 'Housekeeping' || role === 'Admin';
  const isMaintenance = role === 'Maintenance' || role === 'Admin';

  return (
    <Tab.Navigator
      screenOptions={({ route }) => ({
        tabBarIcon: ({ focused, color, size }) => {
          let iconName;
          if (route.name === 'Dashboard') {
            iconName = focused ? 'speedometer' : 'speedometer-outline';
          } else if (route.name === 'Tasks') {
            iconName = focused ? 'checkbox' : 'checkbox-outline';
          } else if (route.name === 'Issues') {
            iconName = focused ? 'warning' : 'warning-outline';
          } else if (route.name === 'Check') {
            iconName = focused ? 'swap-horizontal' : 'swap-horizontal-outline';
          } else if (route.name === 'Profile') {
            iconName = focused ? 'person' : 'person-outline';
          }
          return <Icon name={iconName!} size={size} color={color} />;
        },
        tabBarActiveTintColor: '#007AFF',
        tabBarInactiveTintColor: 'gray',
      })}
    >
      <Tab.Screen name="Dashboard" component={DashboardScreen} />
      {isHousekeeping && <Tab.Screen name="Tasks" component={TasksStack} options={{ headerShown: false }} />}
      {isMaintenance && <Tab.Screen name="Issues" component={IssuesStack} options={{ headerShown: false }} />}
      {isFrontDesk && <Tab.Screen name="Check" component={CheckStack} options={{ headerShown: false }} />}
      <Tab.Screen name="Profile" component={ProfileStack} options={{ headerShown: false }} />
    </Tab.Navigator>
  );
}

function AppNavigator() {
  const { isAuthenticated, loading } = useStaffAuthContext();

  if (loading) {
    return null;
  }

  return (
    <Stack.Navigator screenOptions={{ headerShown: false }}>
      {!isAuthenticated ? (
        <Stack.Screen name="Login" component={LoginScreen} />
      ) : (
        <Stack.Screen name="Main" component={MainTabs} />
      )}
    </Stack.Navigator>
  );
}

export default function App() {
  return (
    <StaffAuthProvider>
      <NavigationContainer>
        <AppNavigator />
      </NavigationContainer>
    </StaffAuthProvider>
  );
}