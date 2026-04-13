import React from 'react';
import { View, Text, StyleSheet } from 'react-native';

interface Props {
  sentiment: string;
  size?: 'small' | 'medium';
}

export const SentimentBadge: React.FC<Props> = ({ sentiment, size = 'medium' }) => {
  const getSentimentColor = (sentiment: string) => {
    switch (sentiment) {
      case 'Positive': return '#10b981';
      case 'Negative': return '#ef4444';
      case 'Neutral': return '#6b7280';
      default: return '#6b7280';
    }
  };

  const getSentimentIcon = (sentiment: string) => {
    switch (sentiment) {
      case 'Positive': return '😊';
      case 'Negative': return '😞';
      case 'Neutral': return '😐';
      default: return '😐';
    }
  };

  const fontSize = size === 'small' ? 10 : 12;
  const padding = size === 'small' ? 4 : 6;

  return (
    <View style={[styles.badge, { backgroundColor: getSentimentColor(sentiment), paddingHorizontal: padding * 2, paddingVertical: padding }]}>
      <Text style={styles.icon}>{getSentimentIcon(sentiment)}</Text>
      <Text style={[styles.text, { fontSize }]}>{sentiment}</Text>
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
    fontSize: 12,
  },
  text: {
    color: 'white',
    fontWeight: 'bold',
  },
});