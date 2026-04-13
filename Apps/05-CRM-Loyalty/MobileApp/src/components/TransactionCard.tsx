import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { LoyaltyTransaction } from '../types/loyalty';

interface Props {
  transaction: LoyaltyTransaction;
}

export const TransactionCard: React.FC<Props> = ({ transaction }) => {
  const getTypeColor = (type: string) => {
    switch (type) {
      case 'Earn': return '#16a34a';
      case 'Redeem': return '#dc2626';
      case 'Bonus': return '#f59e0b';
      case 'Expire': return '#6b7280';
      default: return '#888';
    }
  };

  const getTypeIcon = (type: string) => {
    switch (type) {
      case 'Earn': return '+';
      case 'Redeem': return '-';
      case 'Bonus': return '🎁';
      case 'Expire': return '⏰';
      default: return '•';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
  };

  const isPositive = transaction.transactionType === 'Earn' || transaction.transactionType === 'Bonus';

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <View style={[styles.typeIcon, { backgroundColor: getTypeColor(transaction.transactionType) + '20' }]}>
          <Text style={[styles.typeIconText, { color: getTypeColor(transaction.transactionType) }]}>
            {getTypeIcon(transaction.transactionType)}
          </Text>
        </View>
        <View style={styles.headerContent}>
          <Text style={styles.source}>{transaction.source}</Text>
          <Text style={styles.description}>{transaction.description}</Text>
        </View>
        <Text style={[styles.points, { color: getTypeColor(transaction.transactionType) }]}>
          {isPositive ? '+' : ''}{transaction.points}
        </Text>
      </View>
      
      <View style={styles.footer}>
        <Text style={styles.date}>{formatDate(transaction.transactionDate)}</Text>
        <Text style={styles.balance}>
          Bakiye: {transaction.pointsAfter} puan
        </Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 12,
    marginHorizontal: 16,
    marginVertical: 6,
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 2,
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  typeIcon: {
    width: 40,
    height: 40,
    borderRadius: 20,
    alignItems: 'center',
    justifyContent: 'center',
    marginRight: 12,
  },
  typeIconText: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  headerContent: {
    flex: 1,
  },
  source: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
  },
  description: {
    fontSize: 12,
    color: '#666',
    marginTop: 2,
  },
  points: {
    fontSize: 16,
    fontWeight: 'bold',
  },
  footer: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 8,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  date: {
    fontSize: 11,
    color: '#888',
  },
  balance: {
    fontSize: 11,
    color: '#888',
  },
});