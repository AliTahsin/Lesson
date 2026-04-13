import React from 'react';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { createStackNavigator } from '@react-navigation/stack';
import Icon from 'react-native-vector-icons/Ionicons';
import { RecommendationScreen } from './src/screens/RecommendationScreen';
import { PredictionScreen } from './src/screens/PredictionScreen';
import { SentimentScreen } from './src/screens/SentimentScreen';

const Tab = createBottomTabNavigator();
const Stack = createStackNavigator();

function RecommendationsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Recommendations" component={RecommendationScreen} options={{ title: 'Öneriler' }} />
    </Stack.Navigator>
  );
}

function PredictionsStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Predictions" component={PredictionScreen} options={{ title: 'Tahminler' }} />
    </Stack.Navigator>
  );
}

function SentimentStack() {
  return (
    <Stack.Navigator>
      <Stack.Screen name="Sentiment" component={SentimentScreen} options={{ title: 'Duygu Analizi' }} />
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
            if (route.name === 'Recommendations') {
              iconName = focused ? 'bulb' : 'bulb-outline';
            } else if (route.name === 'Predictions') {
              iconName = focused ? 'analytics' : 'analytics-outline';
            } else if (route.name === 'Sentiment') {
              iconName = focused ? 'chatbubble' : 'chatbubble-outline';
            }
            return <Icon name={iconName!} size={size} color={color} />;
          },
          tabBarActiveTintColor: '#007AFF',
          tabBarInactiveTintColor: 'gray',
        })}
      >
        <Tab.Screen name="Recommendations" component={RecommendationsStack} options={{ headerShown: false }} />
        <Tab.Screen name="Predictions" component={PredictionsStack} options={{ headerShown: false }} />
        <Tab.Screen name="Sentiment" component={SentimentStack} options={{ headerShown: false }} />
      </Tab.Navigator>
    </NavigationContainer>
  );
}