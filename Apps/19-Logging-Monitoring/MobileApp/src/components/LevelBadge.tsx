import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface Props {
  level: string;
  size?: 'small' | 'medium';
}

export const LevelBadge: React.FC<Props> = ({ level, size = 'medium' }) => {
  const getLevelColor = (level: string) => {
    switch (level) {
      case 'Error': return '#ef4444';
      case 'Warning': return '#f59e0b';
      case 'Information': return '#10b981';
      case 'Debug': return '#6b7280';
      default: return '#6b7280';
    }
  };

  const getLevelIcon = (level: string) => {
    switch (level) {
      case 'Error': return '❌';
      case 'Warning': return '⚠️';
      case 'Information': return 'ℹ️';
      case 'Debug': return '🐛';
      default: return '📋';
    }
  };

  const fontSize = size === 'small' ? 10 : 12;
  const padding = size === 'small' ? 4 : 6;

  return (
    <View style={[styles.badge, { backgroundColor: getLevelColor(level), paddingHorizontal: padding * 2, paddingVertical: padding }]}>
      <Text style={styles.icon}>{getLevelIcon(level)}</Text>
      <Text style={[styles.text, { fontSize }]}>{level}</Text>
    </View>
  );
};

const styles = StyleSheet.create({
  badge: {
    flexDirection: 'row',
    alignItems: 'center',
    borderRadius: 12,
    gap: 4,
  },
  icon: {
    fontSize: 10,
  },
  text: {
    color: 'white',
    fontWeight: 'bold',
  },
});