import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';

interface Props {
  isHealthy: boolean;
  size?: 'small' | 'medium';
}

export const HealthBadge: React.FC<Props> = ({ isHealthy, size = 'medium' }) => {
  const getColor = () => isHealthy ? '#10b981' : '#ef4444';
  const getText = () => isHealthy ? 'Sağlıklı' : 'Sağlıksız';
  const getIcon = () => isHealthy ? 'checkmark-circle' : 'close-circle';

  const paddingHorizontal = size === 'small' ? 8 : 12;
  const paddingVertical = size === 'small' ? 4 : 6;
  const fontSize = size === 'small' ? 10 : 12;

  return (
    <View style={[styles.badge, { backgroundColor: getColor(), paddingHorizontal, paddingVertical }]}>
      <Icon name={getIcon()} size={fontSize + 2} color="white" />
      <Text style={[styles.text, { fontSize }]}>{getText()}</Text>
    </View>
  );
};

const styles = StyleSheet.create({
  badge: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 20,
    gap: 4,
  },
  text: {
    color: 'white',
    fontWeight: 'bold',
  },
});