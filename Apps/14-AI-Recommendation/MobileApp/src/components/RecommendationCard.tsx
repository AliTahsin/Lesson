import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { Recommendation } from '../types/ai';

interface Props {
  recommendation: Recommendation;
  onPress: (itemId: number, itemType: string) => void;
  onBook?: (id: number) => void;
}

export const RecommendationCard: React.FC<Props> = ({ recommendation, onPress, onBook }) => {
  const getItemTypeIcon = (type: string) => {
    switch (type) {
      case 'Hotel': return '🏨';
      case 'Room': return '🛏️';
      case 'Restaurant': return '🍽️';
      case 'Event': return '🎉';
      default: return '📍';
    }
  };

  const getAlgorithmBadgeColor = (algorithm: string) => {
    switch (algorithm) {
      case 'CollaborativeFiltering': return '#3b82f6';
      case 'ContentBased': return '#10b981';
      case 'Popular': return '#f59e0b';
      default: return '#6b7280';
    }
  };

  const getScoreColor = (score: number) => {
    if (score >= 0.8) return '#10b981';
    if (score >= 0.6) return '#f59e0b';
    return '#ef4444';
  };

  const scorePercentage = Math.round(recommendation.score * 100);

  return (
    <TouchableOpacity
      style={styles.card}
      onPress={() => onPress(recommendation.itemId, recommendation.itemType)}
    >
      <View style={styles.header}>
        <View style={styles.typeIcon}>
          <Text style={styles.typeIconText}>{getItemTypeIcon(recommendation.itemType)}</Text>
        </View>
        <View style={styles.headerInfo}>
          <Text style={styles.itemName}>{recommendation.itemName || `${recommendation.itemType} ${recommendation.itemId}`}</Text>
          <Text style={styles.reason}>{recommendation.reason}</Text>
        </View>
        <View style={[styles.algorithmBadge, { backgroundColor: getAlgorithmBadgeColor(recommendation.algorithm) }]}>
          <Text style={styles.algorithmText}>
            {recommendation.algorithm === 'CollaborativeFiltering' ? 'CF' :
             recommendation.algorithm === 'ContentBased' ? 'CB' : 'POP'}
          </Text>
        </View>
      </View>

      <View style={styles.footer}>
        <View style={styles.scoreContainer}>
          <View style={[styles.scoreBar, { width: `${scorePercentage}%`, backgroundColor: getScoreColor(recommendation.score) }]} />
          <Text style={[styles.scoreText, { color: getScoreColor(recommendation.score) }]}>
            {scorePercentage}% eşleşme
          </Text>
        </View>
        
        {onBook && !recommendation.isBooked && (
          <TouchableOpacity
            style={styles.bookButton}
            onPress={() => onBook(recommendation.id)}
          >
            <Text style={styles.bookButtonText}>Rezervasyon Yap</Text>
          </TouchableOpacity>
        )}
        
        {recommendation.isBooked && (
          <View style={styles.bookedBadge}>
            <Icon name="checkmark-circle" size={16} color="#10b981" />
            <Text style={styles.bookedText}>Rezervasyon Yapıldı</Text>
          </View>
        )}
      </View>
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    padding: 16,
    elevation: 2,
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 12,
  },
  typeIcon: {
    width: 40,
    height: 40,
    borderRadius: 20,
    backgroundColor: '#f0f0f0',
    justifyContent: 'center',
    alignItems: 'center',
    marginRight: 12,
  },
  typeIconText: {
    fontSize: 20,
  },
  headerInfo: {
    flex: 1,
  },
  itemName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  reason: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  algorithmBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  algorithmText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  footer: {
    marginTop: 8,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  scoreContainer: {
    marginBottom: 12,
  },
  scoreBar: {
    height: 4,
    borderRadius: 2,
    marginBottom: 4,
  },
  scoreText: {
    fontSize: 11,
    fontWeight: '500',
  },
  bookButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 10,
    alignItems: 'center',
  },
  bookButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  bookedBadge: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 6,
  },
  bookedText: {
    fontSize: 12,
    color: '#10b981',
    fontWeight: 'bold',
  },
});