import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface Props {
  level: string;
  size?: 'small' | 'large';
}

export const LevelBadge: React.FC<Props> = ({ level, size = 'small' }) => {
  const getLevelConfig = (level: string) => {
    switch (level) {
      case 'Bronze':
        return { color: '#cd7f32', icon: '🥉', gradient: ['#cd7f32', '#b87333'] };
      case 'Silver':
        return { color: '#c0c0c0', icon: '🥈', gradient: ['#c0c0c0', '#a8a8a8'] };
      case 'Gold':
        return { color: '#ffd700', icon: '🥇', gradient: ['#ffd700', '#ffb800'] };
      case 'Platinum':
        return { color: '#e5e4e2', icon: '💎', gradient: ['#e5e4e2', '#c0c0c0'] };
      case 'Diamond':
        return { color: '#b9f2ff', icon: '👑', gradient: ['#b9f2ff', '#7dd3fc'] };
      default:
        return { color: '#888', icon: '⭐', gradient: ['#888', '#666'] };
    }
  };

  const config = getLevelConfig(level);
  const fontSize = size === 'large' ? 24 : 14;
  const padding = size === 'large' ? 8 : 4;

  return (
    <View style={[styles.badge, { backgroundColor: config.color, paddingHorizontal: padding * 2, paddingVertical: padding }]}>
      <Text style={[styles.icon, { fontSize }]}>{config.icon}</Text>
      <Text style={[styles.levelText, { fontSize: fontSize * 0.8 }]}>{level}</Text>
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
  icon: {
    marginRight: 4,
  },
  levelText: {
    fontWeight: 'bold',
    color: '#333',
  },
});